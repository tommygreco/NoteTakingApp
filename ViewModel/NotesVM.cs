using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteClone.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
		// List of notebooks retrieved from the database.
        public ObservableCollection<Notebook> Notebooks { get; set; }

		// List of notes retrieved from the database.
        public ObservableCollection<Note> Notes { get; set; }

		// Currently selected note.
		private Note selectedNote;
		public Note SelectedNote
		{
			get { return selectedNote; }
			set 
			{
				selectedNote = value;
				OnPropertyChanged("SelectedNote");
				SelectedNoteChanged?.Invoke(this, new EventArgs());
			}
		}

		// Currently selected notebook.
		private Notebook selectedNotebook;
        public Notebook SelectedNotebook
		{
			get { return selectedNotebook; }
			set
			{ 
				selectedNotebook = value;
				OnPropertyChanged("SelectedNotebook");
				GetNotes();
			}
		}

		// Used for showing the textbox when renaming a notebook.
		private Visibility isVisible;
		public Visibility IsVisible
		{
			get { return isVisible; }
			set
			{
				isVisible = value;
                OnPropertyChanged("IsVisible");
            }
			
		}

		// Commands
		public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
		public EditCommand EditCommand { get; set; }
		public EndEditingCommand EndEditingCommand { get; set; }

		// Event Handlers.
        public event PropertyChangedEventHandler? PropertyChanged;
		public event EventHandler? SelectedNoteChanged;

        public NotesVM()
		{
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
			EditCommand = new EditCommand(this);
			EndEditingCommand = new EndEditingCommand(this);

			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();

			IsVisible = Visibility.Collapsed;

			GetNotebooks();
		}

		// Creates a new note for the selected notebook and inserts into the database.
		public async void CreateNote(string notebookId)
		{
			Note newNote = new Note()
			{
				NotebookId = notebookId,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Title = $"Note for {DateTime.Now.ToString()}"
			};

            await DatabaseHelper.Insert(newNote);
			selectedNote = newNote;
			GetNotes();
        }

		// Creates a new notebook for the user and inserts into the database.
		public async void CreateNotebook()
		{
			Notebook newNotebook = new Notebook()
			{
				Name = "New Notebook",
				UserId = App.UserId
			};

			await DatabaseHelper.Insert(newNotebook);
			GetNotebooks();
		}

		// Retrieves notebooks from the database for the current user and adds them to the observable collection.
		public async void GetNotebooks()
		{
			List<Notebook> notebooks = (await DatabaseHelper.Read<Notebook>()).Where(n => n.UserId == App.UserId).ToList();
			Notebooks.Clear();
			foreach (Notebook nb in notebooks)
			{
				Notebooks.Add(nb);
			}
		}

        // Retrieves notes from the database for the current notebook and adds them to the observable collection.
        private async void GetNotes()
        {
			if (SelectedNotebook != null)
			{
                var noteList = (await DatabaseHelper.Read<Note>()).Where(n => n.NotebookId == SelectedNotebook.Id);
                Notes.Clear();
                foreach (Note curNote in noteList)
                {
                    Notes.Add(curNote);
                }
            }
        }

		// Updates subscribers when a property is changed.
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// Makes the textbox visible when editing a notebook name.
		public void StartEditing()
		{
			IsVisible = Visibility.Visible;
		}

        // Makes the textbox collapsed when finished editing a notebook name.
        public void StopEditing(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;
			DatabaseHelper.Update(notebook);
			GetNotebooks();
        }
    }
}
