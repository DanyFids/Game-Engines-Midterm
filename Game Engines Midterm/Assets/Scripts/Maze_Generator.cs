using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Generator : MonoBehaviour
{
	enum TileType {
		UNDEFINED,
		SPAWN,
		END,
		CHECKPOINT
	}

	enum Direction
	{
		NONE,
		NORTH,
		EAST,
		SOUTH,
		WEST
	}

	class Room
	{
		public static Room SPAWN_ROOM;

		public Room north = null;
		public Room east = null;
		public Room south = null;
		public Room west = null;

		TileType _tileType;
		Vector2Int _grid_pos;

		bool _isBuilt = false;
		bool _searched = false;
		bool _dead_end = false;

		public Room(TileType t, Vector2Int pos)
		{
			_tileType = t;
			_grid_pos = pos;
		}

		public Room(Vector2Int pos)
		{
			_tileType = 0;
			_grid_pos = pos;
		}

		public Room Find(Vector2 pos, Direction enter = Direction.NONE) {
			_searched = true;
			Room ret = null;

			if (_grid_pos.x == pos.x && _grid_pos.y == pos.y)
				ret = this;
			if (ret == null && north != null && !north.Searched())
				ret = north.Find(pos, Direction.NORTH);
			if (ret == null && east != null && !east.Searched())
				ret = east.Find(pos, Direction.EAST);
			if (ret == null && south != null && !south.Searched())
				ret = south.Find(pos, Direction.SOUTH);
			if (ret == null && west != null && !west.Searched())
				ret = west.Find(pos, Direction.WEST);

			_searched = false;
			return ret;
		}

		public Vector2Int GetPos()
		{
			return _grid_pos;
		}

		public bool Searched() {
			return _searched;
		}

		public bool isDeadEnd()
		{
			return _dead_end;
		}

		public void MarkDeadEnd()
		{
			_dead_end = true;
		}

		public void Build(Maze_Generator gen, Direction enter = Direction.NONE)
		{
			_isBuilt = true;

			Vector3 pos = new Vector3(_grid_pos.x * 10, 5.0f, _grid_pos.y * 10);
			Vector3 rot = new Vector3();
			Queue<Direction> build_queue = new Queue<Direction>();

			switch (_tileType) {
				case TileType.END:
				case TileType.SPAWN:
					if (north != null)
					{
						rot.y = 0;
						build_queue.Enqueue(Direction.NORTH);
					}
					else if (east != null)
					{
						rot.y = 90.0f;
						build_queue.Enqueue(Direction.EAST);
					}
					else if (south != null)
					{
						rot.y = 180.0f;
						build_queue.Enqueue(Direction.SOUTH);
					}
					else if (west != null)
					{
						rot.y = -90.0f;
						build_queue.Enqueue(Direction.WEST);
					}

					if(_tileType == TileType.SPAWN)
						gen.SPAWN_TILE.transform.SetPositionAndRotation(pos, Quaternion.Euler(rot));
					else
						gen.END_TILE.transform.SetPositionAndRotation(pos, Quaternion.Euler(rot));

					break;
				case TileType.CHECKPOINT:
					GameObject.Instantiate(gen.CHECKPOINT_TILE, pos, Quaternion.Euler(rot));
					break;
				case TileType.UNDEFINED:
				default:
					int rand = Random.Range(0, 4);
					rot.y = 90.0f * rand;
					GameObject.Instantiate(gen.BLANK_TILE, pos, Quaternion.Euler(rot));
					break;
			}

			if (_tileType != TileType.SPAWN)
			{
				if (north == null)
				{
					Vector3 wall_pos = pos + new Vector3(0.0f, -3.0f, 5.0f);
					Vector3 wall_rot = new Vector3();

					Room potential_room = SPAWN_ROOM.Find(_grid_pos + new Vector2Int(0, 1), Direction.NONE);
					if (potential_room != null && !potential_room.IsBuilt())
						GameObject.Instantiate(gen.WALL, wall_pos, Quaternion.Euler(wall_rot));
					else if (potential_room == null)
					{
						GameObject.Instantiate(gen.WALL, wall_pos, Quaternion.Euler(wall_rot));
					}
				}
				else
				{
					build_queue.Enqueue(Direction.NORTH);
				}
				if (east == null)
				{
					Vector3 wall_pos = pos + new Vector3(5.0f, -3.0f, 0.0f);
					Vector3 wall_rot = new Vector3(0.0f, 90.0f, 0.0f);
					Room potential_room = SPAWN_ROOM.Find(_grid_pos + new Vector2Int(1, 0), Direction.NONE);
					if (potential_room != null && !potential_room.IsBuilt())
						GameObject.Instantiate(gen.WALL, wall_pos, Quaternion.Euler(wall_rot));
					else if (potential_room == null)
					{
						GameObject.Instantiate(gen.WALL, wall_pos, Quaternion.Euler(wall_rot));
					}
				}
				else
				{
					build_queue.Enqueue(Direction.EAST);
				}
				if (south == null)
				{
					Vector3 wall_pos = pos + new Vector3(0.0f, -3.0f, -5.0f);
					Vector3 wall_rot = new Vector3(0.0f, 180.0f, 0.0f);
					Room potential_room = SPAWN_ROOM.Find(_grid_pos + new Vector2Int(0, -1), Direction.NONE);
					if (potential_room != null && !potential_room.IsBuilt())
						GameObject.Instantiate(gen.WALL, wall_pos, Quaternion.Euler(wall_rot));
					else if (potential_room == null)
					{
						GameObject.Instantiate(gen.WALL, wall_pos, Quaternion.Euler(wall_rot));
					}
				}
				else
				{
					build_queue.Enqueue(Direction.SOUTH);
				}
				if (west == null)
				{
					Vector3 wall_pos = pos + new Vector3(-5.0f, -3.0f, 0.0f);
					Vector3 wall_rot = new Vector3(0.0f, -90.0f, 0.0f);
					Room potential_room = SPAWN_ROOM.Find(_grid_pos + new Vector2Int(-1, 0), Direction.NONE);
					if (potential_room != null && !potential_room.IsBuilt())
						GameObject.Instantiate(gen.WALL, wall_pos, Quaternion.Euler(wall_rot));
					else if (potential_room == null)
					{
						GameObject.Instantiate(gen.WALL, wall_pos, Quaternion.Euler(wall_rot));
					}
				}
				else
				{
					build_queue.Enqueue(Direction.WEST);
				}
			}

			while (build_queue.Count > 0)
			{
				Direction d = build_queue.Dequeue();
				switch (d)
				{
					default:
					case Direction.NORTH:
						if (!north.IsBuilt())
							north.Build(gen, Direction.NORTH);
						break;
					case Direction.EAST:
						if (!east.IsBuilt())
							east.Build(gen, Direction.EAST);
						break;
					case Direction.SOUTH:
						if (!south.IsBuilt())
							south.Build(gen, Direction.SOUTH);
						break;
					case Direction.WEST:
						if (!west.IsBuilt())
							west.Build(gen, Direction.WEST);
						break;
				}
			}
		}

		public bool IsBuilt()
		{
			return _isBuilt;
		}

		public int PossiblePaths()
		{
			int count = 0;

			if (SPAWN_ROOM.Find(_grid_pos + new Vector2Int(0, 1)) == null)
				count++;
			if (SPAWN_ROOM.Find(_grid_pos + new Vector2Int(1, 0)) == null)
				count++;
			if (SPAWN_ROOM.Find(_grid_pos + new Vector2Int(0, -1)) == null)
				count++;
			if (SPAWN_ROOM.Find(_grid_pos + new Vector2Int(-1, 0)) == null)
				count++;

			return count;

		}
	}

	public GameObject SPAWN_TILE;
	public GameObject BLANK_TILE;
	public GameObject WALL;
	public GameObject CHECKPOINT_TILE;
	public GameObject END_TILE;

	private static int MIN_PATH = 20;
	private static Vector2Int MAZE_DIMM = new Vector2Int(40, 40);
	private static int NUM_CHECKPOINTS = 5;

	private Room Spawn;
	private Room cur_room;

    // Start is called before the first frame update
    void Start()
    {
		int spawn_x = Random.Range(0, 6);
		int spawn_y = Random.Range(0, 6);

		Spawn = new Room(TileType.SPAWN, new Vector2Int(spawn_x, spawn_y));
		Room.SPAWN_ROOM = Spawn;

		cur_room = Spawn;
		for (int c = 0; c < MIN_PATH+1; c++)
		{
			while (cur_room.PossiblePaths() <= 0)
			{
				cur_room.MarkDeadEnd();

				if (cur_room.north != null && !cur_room.north.isDeadEnd())
					cur_room = cur_room.north;
				else if (cur_room.east != null && !cur_room.east.isDeadEnd())
					cur_room = cur_room.east;
				else if (cur_room.south != null && !cur_room.south.isDeadEnd())
					cur_room = cur_room.south;
				else if (cur_room.west != null && !cur_room.west.isDeadEnd())
					cur_room = cur_room.west;
			}

			bool find_empty = true;
			while (find_empty) {

				int r = Random.Range(0, 4);
				Vector2Int new_room_pos;

				switch (r)
				{
					default:
					case 0:
						new_room_pos = cur_room.GetPos() + new Vector2Int(0, 1);
						break;
					case 1:
						new_room_pos = cur_room.GetPos() + new Vector2Int(1, 0);
						break;
					case 2:
						new_room_pos = cur_room.GetPos() + new Vector2Int(0, -1);
						break;
					case 3:
						new_room_pos = cur_room.GetPos() + new Vector2Int(-1, 0);
						break;
				}

				Room found = Spawn.Find(new_room_pos);

				if (found == null)
				{
					Room new_r = new Room(new_room_pos);

					if (c == MIN_PATH)
					{
						new_r = new Room(TileType.END, new_room_pos);
					}

					switch (r)
					{
					default:
					case 0: // North
						cur_room.north = new_r;
						cur_room.north.south = cur_room;

						cur_room = cur_room.north;

						find_empty = false;
						break;
					case 1: // East
						cur_room.east = new_r;

						cur_room.east.west = cur_room;

						cur_room = cur_room.east;

						find_empty = false;
						break;
					case 2: // South
						cur_room.south = new_r;

						cur_room.south.north = cur_room;

						cur_room = cur_room.south;

						find_empty = false;
						break;
					case 3: // West
						cur_room.west = new_r;

						cur_room.west.east = cur_room;

						cur_room = cur_room.west;

						find_empty = false;
						break;
					}
				}
			}
			
		}
		Spawn.Build(this, Direction.NONE);
	}

	bool OutOfBounds(Vector2Int pos)
	{
		if (pos.x < 0 || pos.x > MAZE_DIMM.x || pos.y < 0 || pos.y > MAZE_DIMM.y)
			return true;
		return false;
	}
}
