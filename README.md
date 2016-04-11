# Guide to development
# 1. Các thuật ngữ trong Unity/Game Dev
* Assets: Các thành phần tạo nên game (Scene, Script, Model, Material, Prefab, ...), quản lý ở panel Project trong Unity.
* Scene: Các cảnh trong game, nơi tập hợp các GameObject.
* GameObject: Các đối tượng trong một Scene.
* Sprite: Ảnh hiển thị của một đối tượng 2D.
* Component: Các thành phần của một GameObject, quản lý ở panel Inspector trong Unity. Một số component cơ bản:
  * Transform: Vị trí, góc xoay, kích thước đối tượng.
  * Mesh: Đa giác bề mặt của đối tượng.
  * Material: Chất liệu làm nên bề mặt đối tượng, có thể là Color hoặc Texture.
  * Collider: Hitbox của đối tượng, dùng trong phát hiện va chạm.
  * RigidBody: Cowchees vật lý gắn lên đối tượng, đem lại các hiệu ứng vật lý cho đối tượng (điển hình là Gravity).
  * Animation: Hoạt cảnh của đối tượng.
  * Script: Script điều khiển đối tượng.
* Prefab: Template chứa sẵn một đối tượng GameObject cùng với các thành phần và thuộc tính định sẵn.
  * VD: Prefab HumanBody được cấu tạo từ các bộ phận + script điều khiển các bộ phận đó. Khi cần tạo một đối tượng HumanBody khác, ta chỉ cần sử dụng prefab HumanBody mà không cần phải xây dựng lại từ đầu các bộ phận và script.
* FOV (Field of View): Phạm vi quan sát của camera (có hình kim tự tháp).
* Collision Detection: Phát hiện va chạm. Có 2 kiểu phát hiện va chạm trong game 3D/2D:
  * Primitive: Dùng khối cơ bản (Box, Sphere, Capsules, ...) làm hitbox - collider cho đối tượng. -> Ít đòi hỏi tính toán ~ không chính xác.
  * Mesh: Dùng chính khối đa giác bề mặt của đối tượng (gọi là Mesh) làm collider. -> Đòi hỏi tính toán nhiều ~ chính xác cao.

# 2. Sample Code
## Transformation
Đoạn code sau khởi tạo đối tượng obj ở vị trí (-5, 0, 0).
obj sẽ di chuyển ngang tới điểm (5, 0, 0) trong 1s và cứ thế di chuyển tiếp đến vô cùng. 
```C#
using UnityEngine;
using System.Collections;

public class SpriteControl : MonoBehaviour {
    public GameObject obj;
    // Use this for initialization
    Vector3 start = new Vector3(-5, 0, 0);
    Vector3 dest = new Vector3(5, 0, 0);
	void Start () {
        obj.transform.position = start;
	}
	
	// Update is called once per frame
	void Update () {
        obj.transform.position += (dest - start) * Time.deltaTime;
	}
}

```
Thêm script vào component của đối tượng (vd: Circle1), sau đó đặt obj là Circle1 thì Circle1 sẽ chuyển động như trên.
## Xử lý Click trên Sprite
Trước tiên, ta thêm thành phần Collider cho object Sprite (có thể chọn loại Collider bất kỳ, VD: Box Collider 2D). Mục đích của bước này là để sử dụng method OnMouseDown.
Đoạn code sau dịch chuyển đối tượng Sprite đến tọa độ 2D x = -2.87, y = -1.02.
> Lưu ý: Tham số của Vector2(float x, float y) ở kiểu float, do đó không thể viết Vector2(-2.87, -1.02) mà phải thêm ký tự "f" vào sau để chỉ định đó là float thay vì mặc định là double.

```C#
using UnityEngine;
using System.Collections;

public class SpriteControl : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        obj.transform.position = start;
    }
	
	// Update is called once per frame
	void Update () {
        
	}
	
	// Click Handler
	void OnMouseDown () {
		transform.position = new Vector2(-2.87f, -1.02f);
	}
}

```
Tham khảo thêm: [MonoBehaviour.OnMouseDown()](http://docs.unity3d.com/ScriptReference/MonoBehaviour.OnMouseDown.html).
## Chuyển Animation Clip
```C#
using UnityEngine;
using System.Collections;

public class OnClick : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start () {
		// Get reference to component Animator of gameObject
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown ()
    {
    	// Play clip named "Animation2"
        anim.Play("Animation2");
    }
}
```
> Lưu ý: Ở đây ta dùng kiểu Animator chứ không phải Animation, vì một gameObject chỉ có thành phần Animator (xem tại Inspector trong Unity Editor) chứ không phải Animation. Dùng GetComponent<Animation>() trong trường hợp này sẽ trả về giá trị **null**.

## Debug Log
```C#
using UnityEngine;
using System.Collections;

public class OnClick : MonoBehaviour {

	void Start () {
		Debug.Log("Game Start!");
	}
	
	...
}
```
Khi preview game bằng nút play trong Unity, ta có thể xem log ở tab Console (nếu chưa hiện có thể vào menu Window -> Console (Ctrl+Shift+C)).
[To be updated]
# 3. Một số method/property
| Method/Property                                          | Giải thích                              |
| -------------------------------------------------------- | --------------------------------------- |
| static GameObject GameObject.Find(string name)           | Tìm GameObject có tên name trong Scene hiện tại (không nên dùng vì sau này đổi tên đối tượng thì sẽ rất phiền phức). |
| T GameObject.GetComponent<T>                             | Trả về Component thuộc loại T của GameObject.<br> VD: obj.GetComponent<Transform>() giống với obj.transform ??? (không nên dùng cái này, truy cập trực tiếp tới property vẫn hay hơn). |
| float Time.deltaTime                                     | Trả về khoảng thời gian của frame mới nhất |
| int Time.frameCount                                      | Số frame đã trôi qua kể từ lúc bắt đầu game |
| void Transform.Translate(Vector3 translation)<br>void Transform.Translate(float x, float y, float z) | Di chuyển đối tượng theo vector translation. |
| void Debug.Log(object message)                           | Output message ra Unity Console (dùng để Debug). |
