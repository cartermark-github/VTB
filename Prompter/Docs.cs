using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Prompter
{
    static class Docs
    {
        private static FlowDocument[] _fd;
        private static int _RtbCnt = 50;
        private static int _PageIdx = 0;
        //private static FlowDocument _fdStyle;

        public static FlowDocument[] Fd { get => _fd; set => _fd = value; }

        //public static FlowDocument FdStyle { get => _fdStyle; set => _fdStyle = value; }


        public static int RtbCnt { get => _RtbCnt; }

        public static int PageIdx
        {
            get { return _PageIdx; }
            set
            {

                if (value < 0)
                {
                    value = _RtbCnt - 1;
                }
                if (value > _RtbCnt - 1)
                {
                    value = 0;
                }
                _PageIdx = value;
               


            }
        }

        

        public static void Initialize()
        {
            
            _fd = new FlowDocument[_RtbCnt];
            //_fdStyle = new FlowDocument();
            //Paragraph p = new Paragraph(new Run("Style"));
            //_fdStyle.Blocks.Add(p);
            _PageIdx = 0;
        }
    }
}
