﻿using beClean.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace beClean.Views.NotConnected
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotConnected : BasePage
    {
        public NotConnected()
        {
            InitializeComponent();
            BindingContext = new NotConnectedVM();
        }
    }
}