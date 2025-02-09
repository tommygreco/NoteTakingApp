using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class NewNoteCommand : ICommand
    {
        public NotesVM VM { get; set; }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public NewNoteCommand(NotesVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object? parameter)
        {
            // Create a new note as long as a notebook is selected.
            if (parameter as Notebook != null)
            {
                return true;
            }
            return false;
        }

        public void Execute(object? parameter)
        {
            // Create a new note.
            Notebook selectedNotebook = parameter as Notebook;
            VM.CreateNote(selectedNotebook.Id);
        }
    }
}
