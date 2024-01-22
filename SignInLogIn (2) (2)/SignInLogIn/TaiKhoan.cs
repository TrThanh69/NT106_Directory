using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuDien
{
    class TaiKhoan
    {
        private string TenTK;
        private string MK;
        public TaiKhoan()
        {

        }
        public TaiKhoan(string TenTK, string MK)
        {
            this.TenTK = TenTK;
            this.MK = MK;
        }

        public string TenTk { get => TenTK; set => TenTK = value; }
        public string Mk { get => MK; set => MK = value; }

    }

}

