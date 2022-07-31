using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TestPlayer : MonoBehaviour
{
    private Transform camTrans; //相机位置
    private Vector3 camOffset; //相机偏移量

    private CharacterController ctrl; //角色控制器

    private float targetBlend; //目标混合值
    private float currentBlend; //当前混合值

    private Animator anim;

    protected bool isMove = false; //是否正在移动
    private Vector2 dir = Vector2.zero;

    public float playerMoveSpeed;

    public Vector2 Dir //方向
    {
        get { return dir; }

        set
        {
            if (value == Vector2.zero) //数值为0
            {
                isMove = false; //没有移动
            }
            else //数值不为0
            {
                isMove = true; //正在移动
            }

            dir = value; //方向=数值
        }
    }

    public void Awake()
    {
        anim = GetComponent<Animator>();
        ctrl = GetComponent<CharacterController>();
        camTrans = Camera.main.transform; //相机位置
    }

    private void Update()
    {
        #region Input 输入 测试

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 _dir = new Vector2(h, v).normalized; //方向
        if (_dir != Vector2.zero) //方向不为0
        {
            Dir = _dir;
        }
        else
        {
            Dir = Vector2.zero;
        }

        #endregion

        if (isMove)
        {
            SetDir();
            SetMove();
        }
    }

    private void SetDir() //设置方向
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y; //夹角=（目标角度，当前角度）
        Vector3 eulerAngles = new Vector3(0, angle, 0); //欧拉角，夹角赋值给y
        transform.localEulerAngles = eulerAngles; //局部欧拉角=欧拉角
    }

    private void SetMove() //产生移动
    {
        ctrl.Move(transform.forward * Time.deltaTime * playerMoveSpeed);
    }

    public void Skill()
    {
        if (Input.GetMouseButton(0))
        {
            anim.SetTrigger("Attack");
        }
    }
}