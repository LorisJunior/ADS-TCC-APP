using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TCCApp.Helpers
{
    public interface ISetStatusBarColor
    {
        void SetStatusBarColor(Color color, bool darkTheme);
    }
}
