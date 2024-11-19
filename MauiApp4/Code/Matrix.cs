
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp4.Code
{
    internal class Matrixindice
    {
       private Editor input;

        public Matrixindice( )
        {
            this.Input = new Editor();
            this.Input.Text=0.ToString();
        }

        public Editor Input { get => input; set => input = value; }

        void SetInput(int value)
        {
            this.Input.Text = value.ToString();
        }
    }
}
