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
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }

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


		public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
		public EditCommand EditCommand { get; set; }
		public EndEditingCommand EndEditingCommand { get; set; }

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

		public async void GetNotebooks()
		{
			List<Notebook> notebooks = (await DatabaseHelper.Read<Notebook>()).Where(n => n.UserId == App.UserId).ToList();
			Notebooks.Clear();
			foreach (Notebook nb in notebooks)
			{
				Notebooks.Add(nb);
			}
		}

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

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void StartEditing()
		{
			IsVisible = Visibility.Visible;
		}

        public void StopEditing(Notebook notebook)
        {
            IsVisible = Visibility.Collapsed;
			DatabaseHelper.Update(notebook);
			GetNotebooks();
        }
    }
}
