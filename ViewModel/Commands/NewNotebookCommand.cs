﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class NewNotebookCommand : ICommand
    {
        public NotesVM VM { get; set; }
        public event EventHandler? CanExecuteChanged;

        public NewNotebookCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            // A user can always create a new notebook.
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.CreateNotebook();
        }
    }
}