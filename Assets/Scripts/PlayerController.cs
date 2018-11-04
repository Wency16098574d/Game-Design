using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float rate=5;
    public float timer=0;
    public Bullet bulletPrefab;
    public LayerMask layerMask;
    public Transform bulletPoint;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
 //       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        /*       RaycastHit hitInfo;
               if (Physics.Raycast(ray, out hitInfo, 1000, layerMask))
               {
                   Vector3 target = hitInfo.point;
                   target.y = transform.position.y;
                   transform.LookAt(target);
               }
       */
        Vector3 targetPoint = Camera.main.transform.forward * 100;
        targetPoint.y = transform.position.y;
        transform.LookAt(targetPoint);
        transform.Translate(x, 0, z);
        Shoot();
       
    }


    private void Shoot()
    {
        Vector3 targetPoint;
        if (Input.GetKey(KeyCode.Mouse0))//按下鼠标左键
        {
            timer += Time.deltaTime;//计时器计时
            if (timer > 1f / rate)//如果计时大于子弹的发射速率（rate每秒几颗子弹）
            {
                //通过摄像机在屏幕中心点位置发射一条射线
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))//如果射线碰撞到物体
                {
                    targetPoint = hitInfo.point;//记录碰撞的目标点
                }
                else//射线没有碰撞到目标点
                {
                    //将目标点设置在摄像机自身前方1000米处
                    targetPoint = Camera.main.transform.forward * 1000;
                }
                //在枪口的位置实例化一颗子弹，按子弹发射点出的旋转，进行旋转
                Bullet bullet = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation) as Bullet;
 //               bullet.transform.parent=bulletPoint;
                bullet.transform.LookAt(targetPoint);//子弹的Z轴朝向目标
                Destroy(bullet, 2);//在10S后销毁子弹
                timer = 0;//时间清零
            }
        }
    }

}