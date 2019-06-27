using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TreeViewTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataSource = GetData();
        }
        private ObservableCollection<ExplorerItem> DataSource;
        private ObservableCollection<ExplorerItem> GetData()
        {
            var list = new ObservableCollection<ExplorerItem>();
            ExplorerItem folder1 = new ExplorerItem()
            {
                Name = "Work Documents",
                Type = ExplorerItem.ExplorerItemType.Folder,
                Children =
                {
                    new ExplorerItem()
                    {
                        Name = "Functional Specifications",
                        Type = ExplorerItem.ExplorerItemType.Folder,
                        Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "TreeView spec",
                                Type = ExplorerItem.ExplorerItemType.File,
                              }
                        }
                    },
                    new ExplorerItem()
                    {
                        Name = "Feature Schedule",
                        Type = ExplorerItem.ExplorerItemType.File,
                    },
                    new ExplorerItem()
                    {
                        Name = "Overall Project Plan",
                        Type = ExplorerItem.ExplorerItemType.File,
                    },
                    new ExplorerItem()
                    {
                        Name = "Feature Resources Allocation",
                        Type = ExplorerItem.ExplorerItemType.File,
                    }
                }
            };
            ExplorerItem folder2 = new ExplorerItem()
            {
                Name = "Personal Folder",
                Type = ExplorerItem.ExplorerItemType.Folder,
                Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "Home Remodel Folder",
                                Type = ExplorerItem.ExplorerItemType.Folder,
                                Children =
                                {
                                    new ExplorerItem()
                                    {
                                        Name = "Contractor Contact Info",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Paint Color Scheme",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Flooring Woodgrain type",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Kitchen Cabinet Style",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    }
                                }
                            }
                        }
            };

            list.Add(folder1);
            list.Add(folder2);
            return list;
        }
       
        private void Tree_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Delete)
            {
                 DeleteSelectItem(DataSource);
            }

        }
        private ExplorerItem DeleteSelectItem(ObservableCollection<ExplorerItem> DataSource)
        {
            foreach (var item in DataSource)
            {
                if (item.IsSelected == true)
                {
                    DataSource.Remove(item);
                    return item;
                }

                var FindResult = DeleteSelectItem(item.Children);
                if (FindResult != null)
                    return FindResult;
            }

            return null;
        }
    }
    public class ExplorerItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public enum ExplorerItemType { Folder, File };
        public String Name { get; set; }
        public ExplorerItemType Type { get; set; }
        private ObservableCollection<ExplorerItem> m_children;
        public ObservableCollection<ExplorerItem> Children
        {
            get
            {
                if (m_children == null)
                {
                    m_children = new ObservableCollection<ExplorerItem>();
                }
                return m_children;
            }
            set
            {
                m_children = value;
            }
        }

        private bool m_isExpanded;
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                if (m_isExpanded != value)
                {
                    m_isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        private bool m_isSelected;
        public bool IsSelected
        {
            get { return m_isSelected; }

            set
            {
                if (m_isSelected != value)
                {
                    m_isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }

        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    class ExplorerItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderTemplate { get; set; }
        public DataTemplate FileTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var explorerItem = (ExplorerItem)item;
            return explorerItem.Type == ExplorerItem.ExplorerItemType.Folder ? FolderTemplate : FileTemplate;
        }
    }
}
