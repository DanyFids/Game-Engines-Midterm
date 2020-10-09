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
		public static Vector2Int DIMM_MAX;
		public static Vector2Int DIMM_MIN;

		public Room north = null;
		public Room east = null;
		public Room south = null;
		public Room west = null;

		TileType _tileType;
		Vector2Int _grid_pos;

		int _num_paths;

		bool _isBuilt = false;
		bool _searched = false;
		bool _dead_end = false;

		public Room(TileType t, Vector2Int pos)
		{
			_tileType = t;
			_grid_pos = pos;

			if (pos.x > DIMM_MAX.x)
				DIMM_MAX.x = pos.x;
			else if(pos.x < DIMM_MIN.x)
				DIMM_MIN.x = pos.x;
			if (pos.y > DIMM_MAX.y)
				DIMM_MAX.y = pos.y;
			else if (pos.y < DIMM_MIN.y)
				DIMM_MIN.y = pos.y;

			if (_tileType == TileType.SPAWN || _tileType == TileType.END)
				_num_paths = 1;
		}

		public Room(Vector2Int pos)
		{
			_tileType = 0;
			_grid_pos = pos;

			if (pos.x > DIMM_MAX.x)
				DIMM_MAX.x = pos.x;
			else if (pos.x < DIMM_MIN.x)
				DIMM_MIN.x = pos.x;
			if (pos.y > DIMM_MAX.y)
				DIMM_MAX.y = pos.y;
			else if (pos.y < DIMM_MIN.y)
				DIMM_MIN.y = pos.y;

			_num_paths = Random.Range(0, 5);
			if (_num_paths < 2)
				_num_paths = 2;
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

		public int GetNumPaths()
		{
			return _num_paths;
		}

		public bool isDeadEnd()
		{
			return _dead_end;
		}

		public void MarkDeadEnd()
		{
			_dead_end = true;
		}

		public void SetTileType(TileType t)
		{
			switch (t)
			{
				case TileType.END:
				case TileType.SPAWN:
					_num_paths = 1;
					break;
				case TileType.CHECKPOINT:
					_num_paths = (_num_paths < 4) ? _num_paths + 1 : 4;
					break;
			}
			_tileType = t;
		}

		public Room Simple_Travel(int distance, int move = 0)
		{
			if (move == distance)
				return this;

			_searched = true;

			Room trav = null;
			if (north != null && !north.Searched())
				trav = north;
			else if (east != null && !east.Searched())
				trav = east;
			else if (south != null && !south.Searched())
				trav = south;
			else if (west != null && !west.Searched())
				trav = west;

			Room ret = null;

			if (trav != null)
			{
				ret = trav.Simple_Travel(distance, move + 1);
				_searched = false;
				return ret;
			}
			else
			{
				_searched = false;
				return null;
			}
		}

		public void RecursiveGen(int limit, int travelled = 0)
		{
			_isBuilt = true;
			while (limit > 0 && (_num_paths - ActivePaths()) > 0 && PossiblePaths() > 0)
			{
				int r = Random.Range(0, 4);
				Vector2Int new_room_pos;

				switch (r)
				{
					default:
					case 0:
						new_room_pos = _grid_pos + new Vector2Int(0, 1);
						break;
					case 1:
						new_room_pos = _grid_pos + new Vector2Int(1, 0);
						break;
					case 2:
						new_room_pos = _grid_pos + new Vector2Int(0, -1);
						break;
					case 3:
						new_room_pos = _grid_pos + new Vector2Int(-1, 0);
						break;
				}

				Room found = Room.SPAWN_ROOM.Find(new_room_pos);

				if (found == null)
				{
					Room new_r = new Room(new_room_pos);

					switch (r)
					{
						default:
						case 0: // North
							north = new_r;
							north.south = this;

							break;
						case 1: // East
							east = new_r;
							east.west = this;

							break;
						case 2: // South
							south = new_r;
							south.north = this;

							break;
						case 3: // West
							west = new_r;
							west.east = this;

							break;
					}

					int rand = Random.Range(0, 4);
					int closest = SearchClosest(TileType.CHECKPOINT);
					if (rand == 0 && closest > 4)
					{
						new_r.SetTileType(TileType.CHECKPOINT);
					}
				}
			}


			if (north != null && !north.IsBuilt()) {
				north.RecursiveGen(limit - 1, travelled + 1);
			}
			if (east != null && !east.IsBuilt())
			{
				east.RecursiveGen(limit - 1, travelled + 1);
			}
			if (south != null && !south.IsBuilt())
			{
				south.RecursiveGen(limit - 1, travelled + 1);
			}
			if (west != null && !west.IsBuilt())
			{
				west.RecursiveGen(limit - 1, travelled + 1);
			}

			_isBuilt = false;
		}

		public void Build(Maze_Generator gen, Direction enter = Direction.NONE)
		{
			_isBuilt = true;

			Vector3 pos = new Vector3(_grid_pos.x * 10, 5.0f, _grid_pos.y * 10);
			Vector3 rot = new Vector3();
			Queue<Direction> build_queue = new Queue<Direction>();

			int rand;

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
					rand = Random.Range(0, 4);
					rot.y = 90.0f * rand;
					GameObject.Instantiate(gen.CHECKPOINT_TILE, pos, Quaternion.Euler(rot));
					break;
				case TileType.UNDEFINED:
				default:
					rand = Random.Range(0, 4);
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

		public int ActivePaths() {
			int count = 0;
			if (north != null)
				count++;
			if (east != null)
				count++;
			if (south != null)
				count++;
			if (west != null)
				count++;

			return count;
		}

		// Search Functions
		public int SearchClosest(TileType t)
		{
			if (_tileType == t)
				return 0;
			_searched = true;
			int n, e, s, w;
			n = e = s = w = 100;
			if (north != null && !north.Searched())
				n = north.SearchClosest(t);
			if (east != null && !east.Searched())
				e = east.SearchClosest(t);
			if (south != null && !south.Searched())
				s = south.SearchClosest(t);
			if (west != null && !west.Searched())
				w = west.SearchClosest(t);

			int min = (n < e) ? n : e;
			min = (min < s) ? min : s;
			min = (min < w) ? min : w;

			_searched = false;
			return min + 1;
		}

		public Vector2Int SearchMax()
		{
			_searched = true;

			Vector2Int hold;

			hold = new Vector2Int(int.MinValue, int.MinValue);

			Vector2Int ret = new Vector2Int(_grid_pos.x, _grid_pos.y);

			if (north != null && !north.Searched())
			{
				hold = north.SearchMax();
				ret.x = (ret.x > hold.x) ? ret.x : hold.x;
				ret.y = (ret.y > hold.y) ? ret.y : hold.y;
			}
			if (east != null && !east.Searched())
			{
				hold = east.SearchMax();
				ret.x = (ret.x > hold.x) ? ret.x : hold.x;
				ret.y = (ret.y > hold.y) ? ret.y : hold.y;
			}
			if (south != null && !south.Searched())
			{
				hold = south.SearchMax();
				ret.x = (ret.x > hold.x) ? ret.x : hold.x;
				ret.y = (ret.y > hold.y) ? ret.y : hold.y;
			}
			if (west != null && !west.Searched())
			{
				hold = west.SearchMax();
				ret.x = (ret.x > hold.x) ? ret.x : hold.x;
				ret.y = (ret.y > hold.y) ? ret.y : hold.y;
			}

			_searched = false;
			return ret;
		}

		public Vector2Int SearchMin()
		{
			_searched = true;

			Vector2Int hold;

			hold = new Vector2Int(int.MaxValue, int.MaxValue);

			Vector2Int ret = new Vector2Int(_grid_pos.x, _grid_pos.y);

			if (north != null && !north.Searched())
			{
				hold = north.SearchMax();
				ret.x = (ret.x < hold.x) ? ret.x : hold.x;
				ret.y = (ret.y < hold.y) ? ret.y : hold.y;
			}
			if (east != null && !east.Searched())
			{
				hold = east.SearchMax();
				ret.x = (ret.x < hold.x) ? ret.x : hold.x;
				ret.y = (ret.y < hold.y) ? ret.y : hold.y;
			}
			if (south != null && !south.Searched())
			{
				hold = south.SearchMax();
				ret.x = (ret.x < hold.x) ? ret.x : hold.x;
				ret.y = (ret.y < hold.y) ? ret.y : hold.y;
			}
			if (west != null && !west.Searched())
			{
				hold = west.SearchMax();
				ret.x = (ret.x < hold.x) ? ret.x : hold.x;
				ret.y = (ret.y < hold.y) ? ret.y : hold.y;
			}

			_searched = false;
			return ret;
		}
	}

	public GameObject SPAWN_TILE;
	public GameObject BLANK_TILE;
	public GameObject WALL;
	public GameObject CHECKPOINT_TILE;
	public GameObject END_TILE;

	private static int MIN_PATH = 30;
	private static Vector2Int MAZE_DIMM = new Vector2Int(40, 40);
	private static int NUM_CHECKPOINTS = 5;

	private Room Spawn;
	private Room End;
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
						End = new_r;
					}
					else if (c % 5 == 0 && c != 0)
					{
						new_r.SetTileType(TileType.CHECKPOINT);
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

		Room Rec_start = null;

		if (Spawn.north != null)
		{
			Rec_start = Spawn.north;
		}
		else if (Spawn.east != null)
		{
			Rec_start = Spawn.east;
		}
		else if (Spawn.south != null)
		{
			Rec_start = Spawn.south;
		}
		else if (Spawn.west != null)
		{
			Rec_start = Spawn.west;
		}

		if(Rec_start != null)
			Rec_start.RecursiveGen(MIN_PATH / 2);

		Room extra_start = End.Simple_Travel(6);
		extra_start.RecursiveGen(MIN_PATH / 5);

		Vector2Int dimm_max = Spawn.SearchMax();
		Vector2Int dimm_min = Spawn.SearchMin();

		Debug.Log(Spawn.GetPos().ToString());
		Debug.Log(Room.DIMM_MAX.ToString());
		Debug.Log(Room.DIMM_MIN.ToString());

		Spawn.Build(this, Direction.NONE);
	}

	bool OutOfBounds(Vector2Int pos)
	{
		if (pos.x < 0 || pos.x > MAZE_DIMM.x || pos.y < 0 || pos.y > MAZE_DIMM.y)
			return true;
		return false;
	}
}
