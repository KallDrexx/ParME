using System;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using ImGuiHandler;
using Parme.Core;
using Parme.Editor.AppOperations;
using Parme.Editor.Ui.Elements;

namespace Parme.Editor.Ui
{
    public class EditorUiController
    {
        private readonly ImGuiManager _imGuiManager;
        private readonly DemoWindow _imguiDemoWindow;
        private readonly EmitterSettingsController _emitterSettingsController;
        private readonly AppToolbar _appToolbar;
        private readonly NewFileDialog _newFileDialog;
        private readonly AppOperationQueue _appOperationQueue;
        private readonly MessagePopup _messagePopup;

        public bool AcceptingKeyboardInput => _imGuiManager.AcceptingKeyboardInput;
        public bool AcceptingMouseInput => _imGuiManager.AcceptingMouseInput;
        public Vector3 BackgroundColor => _emitterSettingsController.BackgroundColor;

        public EditorUiController(ImGuiManager imGuiManager, 
            SettingsCommandHandler commandHandler, 
            AppOperationQueue appOperationQueue)
        {
            _imGuiManager = imGuiManager;
            _appOperationQueue = appOperationQueue;

            _imguiDemoWindow = new DemoWindow{IsVisible = false};
            _imGuiManager.AddElement(_imguiDemoWindow);
            
            _appToolbar = new AppToolbar();
            _imGuiManager.AddElement(_appToolbar);

            _newFileDialog = new NewFileDialog();
            _newFileDialog.CreateButtonClicked += NewFileDialogOnCreateButtonClicked;
            _imGuiManager.AddElement(_newFileDialog);
            
            _messagePopup = new MessagePopup();
            _imGuiManager.AddElement(_messagePopup);
            
            _emitterSettingsController = new EmitterSettingsController(imGuiManager, commandHandler);
            _appToolbar.NewMenuItemClicked += AppToolbarOnNewMenuItemClicked;
            _appToolbar.OpenMenuItemClicked += AppToolbarOnOpenMenuItemClicked;
        }

        public void Update()
        {
            _emitterSettingsController.Update();
        }

        public void ToggleImGuiDemoWindow()
        {
            _imguiDemoWindow.IsVisible = !_imguiDemoWindow.IsVisible;
        }

        public void WindowResized(int width, int height)
        {
            _emitterSettingsController.ViewportResized(width, height);
        }

        public void NewEmitterSettingsLoaded(EmitterSettings settings, string filename = null)
        {
            _emitterSettingsController.LoadNewSettings(settings);
            _newFileDialog.ClosePopup();
            _appToolbar.CurrentlyOpenFileName = filename;
        }

        public void DisplayErrorMessage(string error)
        {
            // If a dialog is open, then most likely the error is specific to that dialog
            if (_newFileDialog.DialogIsOpen)
            {
                _newFileDialog.ErrorMessage = error;
            }
            else
            {
                _messagePopup.Display(error);
            }
        }

        private void AppToolbarOnNewMenuItemClicked(object sender, EventArgs e)
        {
            _newFileDialog.OpenPopup();
        }

        private void NewFileDialogOnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_newFileDialog.NewFileName) || 
                string.IsNullOrWhiteSpace(Path.GetExtension(_newFileDialog.NewFileName)))
            {
                _newFileDialog.ErrorMessage = "A valid path to a file is required";
                return;
            }
            
            _appOperationQueue.Enqueue(new NewEmitterRequested(_newFileDialog.NewFileName, _newFileDialog.SelectedTemplate));
        }

        private void AppToolbarOnOpenMenuItemClicked(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = App.DefaultExtension,
                Filter = $"Particle Emitter Definition|*{App.DefaultExtension}",
            };

            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _appOperationQueue.Enqueue(new OpenEmitterRequested(dialog.FileName));
            }
        }
    }
}