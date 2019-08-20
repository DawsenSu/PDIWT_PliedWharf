﻿using System;
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

namespace PDIWT_PiledWharf_Core
{
    /// <summary>
    /// Interaction logic for AttachBCInstanceUserControl.xaml
    /// </summary>
    public partial class AttachBCInstanceUserControl : UserControl
    {
        public AttachBCInstanceUserControl()
        {
            InitializeComponent();
            var _locator = new ViewModel.ViewModelLocator();
            DataContext = _locator.AttachBCInstance;
        }
    }
}
