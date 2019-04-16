using System;
using System.Collections.Generic;
using System.Text;

namespace Exam.Model
{
    public class MasterNavigationItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type Target { get; set; }
    }
}
