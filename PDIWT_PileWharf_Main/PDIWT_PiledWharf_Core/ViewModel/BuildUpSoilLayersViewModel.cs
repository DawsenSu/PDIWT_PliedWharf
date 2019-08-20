﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

using System.ComponentModel;
using System.Collections.ObjectModel;
using PDIWT.Formulas;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Collections.Specialized;
namespace PDIWT_PiledWharf_Core.ViewModel
{
    public class BuildUpSoilLayersViewModel : ViewModelBase
    {
        public BuildUpSoilLayersViewModel()
        {
            SoilLayerInfos = new ObservableCollection<PDIWT_BearingCapacity_SoilLayerInfo>();

        }

        /// <summary>
        /// Property Description
        /// </summary>
        public ObservableCollection<PDIWT_BearingCapacity_SoilLayerInfo> SoilLayerInfos { get; set; }


        private PDIWT_BearingCapacity_SoilLayerInfo _selectedSoilLayer;
        /// <summary>
        /// Property Description
        /// </summary>
        public PDIWT_BearingCapacity_SoilLayerInfo SelectedSoilLayer
        {
            get { return _selectedSoilLayer; }
            set { Set(ref _selectedSoilLayer, value); }
        }

        private RelayCommand<DataGridRowEditEndingEventArgs> _addNewSoilLayer;

        /// <summary>
        /// Gets the AddNewSoilLayer.
        /// </summary>
        public RelayCommand<DataGridRowEditEndingEventArgs> AddNewSoilLayer
        {
            get
            {
                return _addNewSoilLayer
                    ?? (_addNewSoilLayer = new RelayCommand<DataGridRowEditEndingEventArgs>(
                    p =>
                    {
                        var _item = p.Row.Item;
                        if (_item is PDIWT_BearingCapacity_SoilLayerInfo)
                        {
                            var _newSoilLayer = _item as PDIWT_BearingCapacity_SoilLayerInfo;

                            if (p.EditAction == DataGridEditAction.Commit)
                            {
                                _isReadyForAction = true;

                                if (string.IsNullOrEmpty(_newSoilLayer.SoilLayerNumber))
                                {
                                    MessageBox.Show($"Newly adding soil number {_newSoilLayer.SoilLayerNumber} doesn't allow to be empty.", "Invalid Soil Layer Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                    p.Cancel = true;
                                    p.Row.Focus();
                                    _isReadyForAction = false;
                                }
                                var _exceptCollection = SoilLayerInfos.Except(new List<PDIWT_BearingCapacity_SoilLayerInfo> { _newSoilLayer });
                                if (_exceptCollection.Contains(_newSoilLayer))
                                {
                                    MessageBox.Show($"Newly adding soil number {_newSoilLayer.SoilLayerNumber} has already existed in current list.", "Duplicated Soil Layer Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                    p.Cancel = true;
                                    p.Row.Focus();
                                    _isReadyForAction = false;
                                }
                            }
                        }

                    }));
            }
        }

        bool _isReadyForAction = true;

        private RelayCommand _confirm;

        /// <summary>
        /// Gets the Confirm.
        /// </summary>
        public RelayCommand Confirm
        {
            get
            {
                return _confirm
                    ?? (_confirm = new RelayCommand(ExecuteConfirm, () => _isReadyForAction));
            }
        }

        private void ExecuteConfirm()
        {
            SoilLayerInfos = new ObservableCollection<PDIWT_BearingCapacity_SoilLayerInfo>(SoilLayerInfos.OrderBy(e => e.SoilLayerNumber));
            // Bearing Capacity ViewModel Register it.
            Messenger.Default.Send(
                new NotificationMessage<ObservableCollection<PDIWT_BearingCapacity_SoilLayerInfo>>(SoilLayerInfos, "Confirm"), "BuildupSoilLayerLib");
            //It's own view Register it.
            Messenger.Default.Send(new NotificationMessage("close the Window"), "BuildUpWindowConfirmClicked");
        }

        private RelayCommand _cancel;

        /// <summary>
        /// Gets the Cancel.
        /// </summary>
        public RelayCommand Cancel
        {
            get
            {
                return _cancel
                    ?? (_cancel = new RelayCommand(
                    () =>
                    {
                        Messenger.Default.Send(new NotificationMessage("close the Window"), "BuildUpWindowConfirmClicked");
                    },
                    () => _isReadyForAction));
            }
        }

        private RelayCommand<CancelEventArgs> _mainWindowClosing;

        /// <summary>
        /// Gets the MainWindowClosing.
        /// </summary>
        public RelayCommand<CancelEventArgs> MainWindowClosing
        {
            get
            {
                return _mainWindowClosing
                    ?? (_mainWindowClosing = new RelayCommand<CancelEventArgs>(
                    p =>
                    {
                        if (_isReadyForAction == false)
                        {
                            MessageBox.Show(PDIWT.Resources.Localization.MainModule.Resources.Note_EEIL);
                            p.Cancel = true;
                        }
                    }));
            }
        }

        private RelayCommand _add;

        /// <summary>
        /// Gets the Add.
        /// </summary>
        public RelayCommand Add
        {
            get
            {
                return _add
                    ?? (_add = new RelayCommand(
                    () =>
                    {
                        var _newlayer = new PDIWT_BearingCapacity_SoilLayerInfo();
                        SoilLayerInfos.Add(_newlayer);
                        SelectedSoilLayer = _newlayer;
                    }));
            }
        }

        private RelayCommand _delete;

        /// <summary>
        /// Gets the Delete.
        /// </summary>
        public RelayCommand Delete
        {
            get
            {
                return _delete
                    ?? (_delete = new RelayCommand(
                    () =>
                    {
                        var _temp = SelectedSoilLayer;

                        if (SoilLayerInfos.Count == 1)
                        {
                            SelectedSoilLayer = null;
                        }
                        else
                        {
                            int _indexOfSelectedRow = SoilLayerInfos.IndexOf(SelectedSoilLayer);

                            if (_indexOfSelectedRow == SoilLayerInfos.Count - 1)
                                SelectedSoilLayer = SoilLayerInfos.ElementAt(_indexOfSelectedRow - 1);
                            else
                                SelectedSoilLayer = SoilLayerInfos.ElementAt(_indexOfSelectedRow + 1);
                        }
                        SoilLayerInfos.Remove(_temp);
                    },
                    () => SelectedSoilLayer != null));
            }
        }
    }
}
