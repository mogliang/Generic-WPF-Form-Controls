using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FormControlLib.ControlSinks
{
    public abstract class ControlSinkBase
    {
        public abstract MatchResult MatchTest(FormItemContext context);
        public abstract FrameworkElement CreateControl(FormItemContext context);
    }

    public enum MatchResult
    {
        No = 0,
        NotRecommanded = 4,
        Yes = 8,
        Recommanded = 12
    }
}
