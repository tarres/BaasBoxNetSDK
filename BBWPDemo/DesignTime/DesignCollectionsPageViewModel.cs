﻿using System.Windows.Input;
using BBWPDemo.ViewModels;

namespace BBWPDemo.DesignTime
{
    internal class DesignCollectionsPageViewModel : ICollectionsPageViewModel
    {
        public string Name { get; set; }
        public ICommand Create { get; private set; }
        public ICommand Delete { get; private set; }
    }
}