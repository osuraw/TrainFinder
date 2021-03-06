﻿using System.Windows;
using System.Windows.Controls;
using Desktop.Model;
using Desktop.ViewModels;

namespace Desktop
{
    
    public partial class TrainU : UserControl
    {
        public TrainU()
        {
            InitializeComponent();
            this.DataContext = new TrainVm();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) TrainVm.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) TrainVm.Errors -= 1;
        }
    }
}

//            MessageBox.Show("Update Success", "Informative", MessageBoxButton.OK, MessageBoxImage.Asterisk);
//            MessageBox.Show("Select Train from Table to Update", "Informative", MessageBoxButton.OK, MessageBoxImage.Asterisk);
//            MessageBox.Show("New Station AddedSuccessfully", "Information", MessageBoxButton.OK,MessageBoxImage.Information);
//            MessageBox.Show("Adding Failed", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
//            MessageBox.Show("Select different values to Starting Station and Ending Station", "Information",MessageBoxButton.OK, MessageBoxImage.Information);
//            MessageBox.Show("Select different values to Starting Station and Ending Station", "Information",MessageBoxButton.OK, MessageBoxImage.Information);