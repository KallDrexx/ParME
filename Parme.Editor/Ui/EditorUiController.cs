using System;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using ImGuiHandler;
using Parme.Editor.AppOperations;
using Parme.Editor.Ui.Elements;

namespace Parme.Editor.Ui
{
    public class EditorUiController
    {
        private readonly ImGuiManager _imGuiManager;
        private readonly DemoWindow _imguiDemoWindow;
        private readonly EmitterSettingsController _emitterSettingsController;
        private readonly NewFileDialog _newFileDialog;
        private readonly AppOperationQueue _appOperationQueue;
        private readonly MessagePopup _messagePopup;
        private readonly ApplicationState _applicationState;
        private readonly AppToolbar _appToolbar;

        public bool AcceptingKeyboardInput => _imGuiManager.AcceptingKeyboardInput;
        public bool AcceptingMouseInput => _imGuiManager.AcceptingMouseInput;
        public Vector3 BackgroundColor => _emitterSettingsController.BackgroundColor;

        public EditorUiController(ImGuiManager imGuiManager, 
            SettingsCommandHandler commandHandler, 
            AppOperationQueue appOperationQueue, 
            ApplicationState applicationState)
        {
            _imGuiManager = imGuiManager;
            _appOperationQueue = appOperationQueue;
            _applicationState = applicationState;

            _imguiDemoWindow = new DemoWindow{IsVisible = false};
            _imGuiManager.AddElement(_imguiDemoWindow);
            
            _appToolbar = new AppToolbar();
            _imGuiManager.AddElement(_appToolbar);

            _newFileDialog = new NewFileDialog();
            _newFileDialog.CreateButtonClicked += NewFileDialogOnCreateButtonClicked;
            _newFileDialog.ModalClosed += NewFileDialogOnModalClosed;
            _imGuiManager.AddElement(_newFileDialog);
            
            _messagePopup = new MessagePopup();
            _messagePopup.ModalClosed += MessagePopupOnModalClosed;
            _imGuiManager.AddElement(_messagePopup);
            
            _emitterSettingsController = new EmitterSettingsController(imGuiManager, commandHandler, applicationState);
            _appToolbar.NewMenuItemClicked += AppToolbarOnNewMenuItemClicked;
            _appToolbar.OpenMenuItemClicked += AppToolbarOnOpenMenuItemClicked;
            _appToolbar.SaveMenuItemClicked += AppToolbarOnSaveMenuItemClicked;
        }

        public void Update()
        {
            _emitterSettingsController.Update();

            _appToolbar.CurrentlyOpenFileName = _applicationState.ActiveFileName;
            _appToolbar.UnsavedChangesPresent = _applicationState.HasUnsavedChanges;

            if (!string.IsNullOrWhiteSpace(_applicationState.ErrorMessage))
            {
                // If a dialog is open, then most likely the error is specific to that dialog
                if (_newFileDialog.DialogIsOpen)
                {
                    _newFileDialog.ErrorMessage = _applicationState.ErrorMessage;
                }
                else
                {
                    _messagePopup.Display(_applicationState.ErrorMessage);
                }
            }
        }

        public void ToggleImGuiDemoWindow()
        {
            _imguiDemoWindow.IsVisible = !_imguiDemoWindow.IsVisible;
        }

        public void WindowResized(int width, int height)
        {
            _emitterSettingsController.ViewportResized(width, height);
        }

        private void MessagePopupOnModalClosed(object sender, EventArgs e)
        {
            _appOperationQueue.Enqueue(new ClearErrorMessageRequested());
        }

        private void AppToolbarOnNewMenuItemClicked(object sender, EventArgs e)
        {
            _newFileDialog.OpenPopup();
        }

        private void NewFileDialogOnCreateButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_newFileDialog.NewFileName))
            {
                _newFileDialog.ErrorMessage = "No file name was given";
                return;
            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(_newFileDialog.NewFileName)))
            {
                _newFileDialog.ErrorMessage = "File name must have an extension";
                return;
            }
            
            _appOperationQueue.Enqueue(new NewEmitterRequested(_newFileDialog.NewFileName, _newFileDialog.SelectedTemplate));
        }

        private void NewFileDialogOnModalClosed(object sender, EventArgs e)
        {
            _appOperationQueue.Enqueue(new ClearErrorMessageRequested());
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

        private void AppToolbarOnSaveMenuItemClicked(object sender, bool selectFileName)
        {
            var filename = _applicationState.ActiveFileName;
            if (selectFileName)
            {
                var dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = App.DefaultExtension,
                    Filter = $"Particle Emitter Definition|*{App.DefaultExtension}",
                    OverwritePrompt = true,
                    FileName = Path.GetFileName(_applicationState.ActiveFileName),
                    InitialDirectory = Path.GetDirectoryName(_applicationState.ActiveFileName),
                };

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                filename = dialog.FileName;
            }
            
            _appOperationQueue.Enqueue(new SaveEmitterRequested(filename, _applicationState.ActiveEmitter));
        }
    }
}