using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Reset_causes_scrolltotop
{
    public class ListViewPageViewModel
    {
        private static readonly IEnumerable<string> _items = Enumerable.Range(0, 50).Select(num => num.ToString());

        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();

        public ICommand ClearListCommand => new Command(_ => { Items.Clear(); });

        public ICommand AddRangeCommand => new Command(_ => AddRange());

        public ICommand AddRangeWithCleanCommand => new Command(_ =>
        {
            Items.Clear();

            for (int i = 0; i < 10; i++)
            {
                AddRange();
            }


        });

        private void AddRange()
        {
            foreach (var item in _items)
            {
                Items.Add(item);
            }
        }

    }
}