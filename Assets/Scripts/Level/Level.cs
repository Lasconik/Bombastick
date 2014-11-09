using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{

		private Tileset tileset;
		private Tilemap map;
		public GameObject tilePrefab;
		static public Vector2 halfVec = Vector2.one / 2;
		private int size;

		void Awake ()
		{
				tileset = new Tileset (WWW.EscapeURL ("Levels/Tilesets/classic.xml"));
				size = tileset.GetTileSize ();
				map = new Tilemap (WWW.EscapeURL ("Levels/classic1.xml"));
				Create ();
		}

		void Create ()
		{
				int stop = map.GetNbLayers ();
				for (int i = 0; i < stop; i++) {
						CreateFloor (i);
				}
		}

		void CreateFloor (int layer)
		{
				int i, j; // iterators for the map
				int w = map.GetWidth ();
				int h = map.GetHeight ();
				// TODO: don't compute this everytime
				Transform tilesObject = transform.Find ("Tiles");
				GameObject layerObject = new GameObject ();
				layerObject.transform.parent = tilesObject;
				layerObject.transform.position = halfVec;
				layerObject.name = "Layer" + layer;
				for (j = 0; j < h; j++) {
						for (i = 0; i < w; i++) {
								int tileIndex = map.Get (i, j, layer);
								GameObject go = CreateTile (tileIndex);
								if (go != null) {
										go.transform.parent = layerObject.transform;
										go.transform.localPosition = new Vector3 (i, h - j, 0);
										go.name = "Tile-" + i + "_" + j + "(" + tileIndex + ")";
										go.renderer.sortingOrder = layer;
								}
						}
				}
		}

		GameObject CreateTile (int tileIndex)
		{
				// retrieving the tile corresponding to the index
				Tile tile = tileset.Get (tileIndex);
				// if in the map, the index is 0, we do not create
				if (tileIndex == 0) {
						return null;
				} else {
						// otherwise, we substract 1 (more convenient for operations)
						tileIndex--;
				}
				// computing subRectangle
				Texture2D texture = tileset.GetTexture ();
				int w = tileset.GetWidth ();
				int h = tileset.GetHeight ();
				int x = (tileIndex % w) * size;
				// TODO: the second parameter could be the top, or the bottom
				int y = texture.height - (tileIndex / w) * size - size;
				// creating the object
				Rect subRect = new Rect (x, y, size, size);
				GameObject go = Instantiate (tilePrefab) as GameObject;
				// assigning sub-sprite
				//TODO : several tiles share the same sprite, no need to create it everytime
				SpriteRenderer sr = (SpriteRenderer)go.renderer;
				sr.sprite = (Sprite.Create (tileset.GetTexture (), subRect, halfVec, size));
				// TODO: colliders and destruction
				return go;
		}

}
