using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicKeyloggerWPF.KeyboardUI
{
    /// <summary>
    /// Interaction logic for FullKeyboard.xaml
    /// </summary>
    public partial class FullKeyboard : UserControl
    {
        private FullKeyboardModel model;
        public FullKeyboardModel Model { get => model; }

        public FullKeyboard()
        {
            InitializeComponent();

            model = new FullKeyboardModel(this);

            oneKey.keyText.Text = "1";
            oneKey.Model.FullKeyboardReference = model;
            twoKey.keyText.Text = "2";
            twoKey.Model.FullKeyboardReference = model;
            threeKey.keyText.Text = "3";
            threeKey.Model.FullKeyboardReference = model;
            fourKey.keyText.Text = "4";
            fourKey.Model.FullKeyboardReference = model;
            fiveKey.keyText.Text = "5";
            fiveKey.Model.FullKeyboardReference = model;
            sixKey.keyText.Text = "6";
            sixKey.Model.FullKeyboardReference = model;
            sevenKey.keyText.Text = "7";
            sevenKey.Model.FullKeyboardReference = model;
            eightKey.keyText.Text = "8";
            eightKey.Model.FullKeyboardReference = model;
            nineKey.keyText.Text = "9";
            nineKey.Model.FullKeyboardReference = model;
            zeroKey.keyText.Text = "0";
            zeroKey.Model.FullKeyboardReference = model;
            dashKey.keyText.Text = "-";
            dashKey.Model.FullKeyboardReference = model;
            equalsKey.keyText.Text = "=";
            equalsKey.Model.FullKeyboardReference = model;
            backspaceKey.keyText.Text = "Backspace";
            backspaceKey.Model.FullKeyboardReference = model;
            tabKey.keyText.Text = "Tab";
            tabKey.Model.FullKeyboardReference = model;
            qKey.keyText.Text = "Q";
            qKey.Model.FullKeyboardReference = model;
            wKey.keyText.Text = "W";
            wKey.Model.FullKeyboardReference = model;
            eKey.keyText.Text = "E";
            eKey.Model.FullKeyboardReference = model;
            rKey.keyText.Text = "R";
            rKey.Model.FullKeyboardReference = model;
            tKey.keyText.Text = "T";
            tKey.Model.FullKeyboardReference = model;
            yKey.keyText.Text = "Y";
            yKey.Model.FullKeyboardReference = model;
            uKey.keyText.Text = "U";
            uKey.Model.FullKeyboardReference = model;
            iKey.keyText.Text = "I";
            iKey.Model.FullKeyboardReference = model;
            oKey.keyText.Text = "O";
            oKey.Model.FullKeyboardReference = model;
            pKey.keyText.Text = "P";
            pKey.Model.FullKeyboardReference = model;
            capsKey.keyText.Text = "Caps";
            capsKey.Model.FullKeyboardReference = model;
            aKey.keyText.Text = "A";
            aKey.Model.FullKeyboardReference = model;
            sKey.keyText.Text = "S";
            sKey.Model.FullKeyboardReference = model;
            dKey.keyText.Text = "D";
            dKey.Model.FullKeyboardReference = model;
            fKey.keyText.Text = "F";
            fKey.Model.FullKeyboardReference = model;
            gKey.keyText.Text = "G";
            gKey.Model.FullKeyboardReference = model;
            hKey.keyText.Text = "H";
            hKey.Model.FullKeyboardReference = model;
            jKey.keyText.Text = "J";
            jKey.Model.FullKeyboardReference = model;
            kKey.keyText.Text = "K";
            kKey.Model.FullKeyboardReference = model;
            lKey.keyText.Text = "L";
            lKey.Model.FullKeyboardReference = model;
            enterKey.keyText.Text = "Enter";
            enterKey.Model.FullKeyboardReference = model;
            lShiftKey.keyText.Text = "L Shift";
            lShiftKey.Model.FullKeyboardReference = model;
            zKey.keyText.Text = "Z";
            zKey.Model.FullKeyboardReference = model;
            xKey.keyText.Text = "X";
            xKey.Model.FullKeyboardReference = model;
            cKey.keyText.Text = "C";
            cKey.Model.FullKeyboardReference = model;
            vKey.keyText.Text = "V";
            vKey.Model.FullKeyboardReference = model;
            bKey.keyText.Text = "B";
            bKey.Model.FullKeyboardReference = model;
            nKey.keyText.Text = "N";
            nKey.Model.FullKeyboardReference = model;
            mKey.keyText.Text = "M";
            mKey.Model.FullKeyboardReference = model;
            lThanKey.keyText.Text = "<";
            lThanKey.Model.FullKeyboardReference = model;
            gThanKey.keyText.Text = ">";
            gThanKey.Model.FullKeyboardReference = model;
            rShiftKey.keyText.Text = "R Shift";
            rShiftKey.Model.FullKeyboardReference = model;
            lCtrlKey.keyText.Text = "L Ctrl";
            lCtrlKey.Model.FullKeyboardReference = model;
            lAltKey.keyText.Text = "L Alt";
            lAltKey.Model.FullKeyboardReference = model;
            spacebarKey.keyText.Text = "Spacebar";
            spacebarKey.Model.FullKeyboardReference = model;
            rAltKey.keyText.Text = "R Alt";
            rAltKey.Model.FullKeyboardReference = model;
            rCtrlKey.keyText.Text = "R Ctrl";
            rCtrlKey.Model.FullKeyboardReference = model;
            rMouseButton.keyText.Text = "RMB";
            rMouseButton.Model.FullKeyboardReference = model;
            lMouseButton.keyText.Text = "LMB";
            lMouseButton.Model.FullKeyboardReference = model;
        }

        private void SavePreset_Click(object sender, RoutedEventArgs e)
        {
            model.SavePreset();
        }

        private void LoadPreset_Click(object sender, RoutedEventArgs e)
        {
            model.LoadPreset();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            model.ResetKeyboard();
        }
    }
}
