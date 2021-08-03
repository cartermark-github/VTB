using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Prompter
{
    class Settings
    {
        private int _TransIdx;
        private int _DelayIdx;
        private String _SaveDir;
        private String _FileName = "VTBSet.data";

        public int TransIdx { get => _TransIdx; set => _TransIdx = value; }
        public int DelayIdx { get => _DelayIdx; set => _DelayIdx = value; }

        private void initialize()
        {
            string folderBase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _SaveDir = String.Format(@"{0}\{1}\", folderBase, "VTB-" + AssemblyGuid);

            if (Directory.Exists(_SaveDir) == false)
            {
                Directory.CreateDirectory(_SaveDir);
            }

        }

        public void SaveSettings()
        {
            initialize();

            using (FileStream fs = new FileStream(_SaveDir + _FileName, FileMode.Create))
            {
                using (BinaryWriter w = new BinaryWriter(fs))
                {
                    w.Write(_TransIdx);
                    w.Write(_DelayIdx);
                }
            }


        }

        public void LoadSettings()
        {
            initialize();

            if (File.Exists(_SaveDir + _FileName))
            {
                using (FileStream fs = new FileStream(_SaveDir + _FileName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader r = new BinaryReader(fs))
                    {
                        _TransIdx = r.ReadInt32();
                        _DelayIdx = r.ReadInt32();
                    }
                }
            }
            else
            {
                _TransIdx = 0;
                _DelayIdx = 1;
            }
        }

        private string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(GuidAttribute), true);
                if (attributes.Length == 0) { return String.Empty; }
                return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value.ToUpper();
            }
        }
    }
}
