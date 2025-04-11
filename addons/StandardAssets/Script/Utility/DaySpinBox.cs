using Godot;
using System;

namespace Game
{

    public partial class DaySpinBox : SpinBox
    {
        [Export] public SpinBox monthSpinBox;

        public override void _Ready()
        {
            if (monthSpinBox != null)
            {
                //monthSpinBox.ValueChanged += OnMonthSpinBoxValueChanged;

            }
        }

        private void OnMonthSpinBoxValueChanged()
        {
            //int 
            //int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, (int)monthSpinBox.month);
            //this.MaxValue = daysInMonth;
        }
    }

}