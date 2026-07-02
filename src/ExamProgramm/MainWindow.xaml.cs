using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ExamProgramm
{
    public partial class MainWindow : Window
    {
    
        private static readonly Person[] _sourceData = new[]
        {
            new Person("Иванов", "Иван", 1985),
            new Person("Петров", "Петр", 1990),
            new Person("Сидоров", "Алексей", 1985),
            new Person("Кузнецов", "Дмитрий", 1978),
            new Person("Смирнова", "Елена", 1995),
            new Person("Васильев", "Олег", 1990),
            new Person("Михайлов", "Андрей", 2000),
            new Person("Новиков", "Игорь", 1985),
            new Person("Федоров", "Кирилл", 1970),
            new Person("Морозов", "Виктор", 1992)
        };

        private Person[]? _persons;
        private PersonnelManagement? _management;
        private int _count = 0;

        public MainWindow()
        {
            InitializeComponent();

            BtnSortAndShow.IsEnabled = false;
            BtnSaveToFile.IsEnabled = false;
            BtnShowUnsorted.IsEnabled = false;
        }

        private void BtnSetSize_Click(object sender, RoutedEventArgs e)
        {
           
            if (!int.TryParse(TxtArraySize.Text, out _count) || _count <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное положительное число.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           
            if (_count > _sourceData.Length)
            {
                _count = _sourceData.Length;
                TxtArraySize.Text = _count.ToString();
                LblStatus.Text = $"Превышено максимальное количество элементов. Установлено: {_count} (всего доступно: {_sourceData.Length})";
            }
            else
            {
                LblStatus.Text = $"Загружено {_count} элементов из исходного массива.";
            }

            try
            {
               
                _persons = new Person[_count];

                Array.Copy(_sourceData, _persons, _count);

                DgUnsorted.ItemsSource = null;
                DgSorted.ItemsSource = null;
 
                _management = null;

               
                BtnShowUnsorted.IsEnabled = true;
                BtnSortAndShow.IsEnabled = true;
                BtnSaveToFile.IsEnabled = false;

               
                DgUnsorted.ItemsSource = _persons;

                LblStatus.Text += " Данные отображены.";
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Произошла критическая ошибка при загрузке данных: {ex.Message}\n\nДетали: {ex.StackTrace}", "Ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnShowUnsorted_Click(object sender, RoutedEventArgs e)
        {
            if (_persons == null || _persons.Length == 0)
            {
                MessageBox.Show("Нет данных для отображения.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DgUnsorted.ItemsSource = _persons;
            LblStatus.Text = "Отображен исходный (неотсортированный) список.";
        }

        private void BtnSortAndShow_Click(object sender, RoutedEventArgs e)
        {
            if (_persons == null || _persons.Length == 0)
            {
                MessageBox.Show("Нет данных для сортировки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _management = new PersonnelManagement(_persons);
                _management.SortByBirthYearAndLastNameDesc();

                var sortedArray = _management.GetPersons();
                DgSorted.ItemsSource = sortedArray;

                LblStatus.Text = "Массив отсортирован: Год рождения (убывание), Фамилия (убывание).";
                BtnSaveToFile.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сортировке: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            if (_management == null)
            {
                MessageBox.Show("Сначала нажмите кнопку 'Отсортировать', чтобы подготовить данные для сохранения.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                FileName = "sorted_personnel.txt",
                Title = "Сохранить отсортированный список"
            };

            if (sfd.ShowDialog() == true)
            {
                try
                {
                    _management.SaveToFile(sfd.FileName);
                    LblStatus.Text = $"Данные успешно сохранены в файл: {Path.GetFileName(sfd.FileName)}";
                    MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось сохранить файл: {ex.Message}", "Ошибка записи", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
