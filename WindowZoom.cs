using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WisdomGrowth
{
 public class WindowZoom
    {

        private float x;//当前窗体的宽度
        private float y;//当前窗体的高度

        public void SetForm(Control form)
        {
            x = form.Width;           
            y = form.Height;
            SetTag(form);
        }

        public void SetReSize(Control form)
        {
            float reSizeX = (form.Width) / x;
            float reSizeY = (form.Height) / y;

            SetControls(reSizeX, reSizeY, form);
        }

        private void SetTag(Control controls)
        {
            foreach (Control ctr in controls.Controls)
            {
                ctr.Tag = ctr.Width + ";" + ctr.Height + ";" + ctr.Left + ";" + ctr.Top + ";" + ctr.Font.Size;
                if (ctr.Controls.Count > 0)
                {
                    SetTag(ctr);
                }
            }
        }

        private void SetControls(float reSizeX, float reSizeY, Control controls)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control ctr in controls.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (ctr.Tag != null)
                {
                    string[] mytag = ctr.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    ctr.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * reSizeX);//宽度
                    ctr.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * reSizeY);//高度
                    ctr.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * reSizeX);//左边距
                    ctr.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * reSizeY);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * reSizeY;//字体大小
                    ctr.Font = new Font(ctr.Font.Name, currentSize, ctr.Font.Style, ctr.Font.Unit);
                    if (ctr.Controls.Count > 0)
                    {
                        SetControls(reSizeX, reSizeY, ctr);
                    }
                }
            }
        }


    }
}