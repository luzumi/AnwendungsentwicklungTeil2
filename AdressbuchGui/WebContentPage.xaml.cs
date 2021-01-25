﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdressbuchGui
{
    /// <summary>
    /// Interaktionslogik für WebContentPage.xaml
    /// </summary>
    public partial class WebContentPage : Page
    {
        

        public WebContentPage(Object pDataContext)
        {
            InitializeComponent();
            DataContext = pDataContext;
        }

        public WebContentPage()
        {
            InitializeComponent();
            DataContext = this.DataContext;
        }
    }
}
