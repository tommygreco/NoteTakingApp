﻿using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }

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

        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public NotesVM()
		{
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);

			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();

			GetNotebooks();
		}

		public void CreateNote(int notebookId)
		{
			Note newNote = new Note()
			{
				NotebookId = notebookId,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Title = $"Note for {DateTime.Now.ToString()}"
			};

            DatabaseHelper.Insert(newNote);
			GetNotes();
        }

		public void CreateNotebook()
		{
			Notebook newNotebook = new Notebook()
			{
				Name = "New Notebook"
			};

			DatabaseHelper.Insert(newNotebook);
			GetNotebooks();
		}

		private void GetNotebooks()
		{
			List<Notebook> notebooks = DatabaseHelper.Read<Notebook>();
			Notebooks.Clear();
			foreach (Notebook nb in notebooks)
			{
				Notebooks.Add(nb);
			}
		}

        private void GetNotes()
        {
			if (SelectedNotebook != null)
			{
                var noteList = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == SelectedNotebook.Id);
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
    }
}
