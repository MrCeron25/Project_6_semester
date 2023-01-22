using Project.Infrastructure.Commands;
using Project.Models;
using Project.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
namespace Project.ViewModels
{
    internal class LoginPageViewModel : ViewModel
    {
        #region Текст логина
        private string _loginText = "Adam@gmail.com";
        public string LoginText
        {
            get => _loginText;
            set => Set(ref _loginText, value);
        }
        #endregion

        #region Название текста изминения капчи
        private string _updateCaptchaText = "Изменить картинку";
        public string UpdateCaptchaText
        {
            get => _updateCaptchaText;
            set => Set(ref _updateCaptchaText, value);
        }
        #endregion

        #region Видимость поля ввода капчи
        private Visibility _сaptchaTextBoxVisibility = Visibility.Hidden;
        public Visibility CaptchaTextBoxVisibility
        {
            get => _сaptchaTextBoxVisibility;
            set => Set(ref _сaptchaTextBoxVisibility, value);
        }
        #endregion

        #region Видимость текста изминения капчи
        private Visibility _updateCaptchaTextBlockVisibility = Visibility.Hidden;
        public Visibility UpdateCaptchaTextBlockVisibility
        {
            get => _updateCaptchaTextBlockVisibility;
            set => Set(ref _updateCaptchaTextBlockVisibility, value);
        }
        #endregion

        #region Видимость текста изминения капчи
        private Visibility _captchaTextVisibility = Visibility.Hidden;
        public Visibility CaptchaTextVisibility
        {
            get => _captchaTextVisibility;
            set => Set(ref _captchaTextVisibility, value);
        }
        #endregion

        #region Видимость капчи
        private Visibility _captchaImageVisibility = Visibility.Hidden;
        public Visibility CaptchaImageVisibility
        {
            get => _captchaImageVisibility;
            set => Set(ref _captchaImageVisibility, value);
        }
        #endregion

        #region Капча
        private System.Windows.Media.ImageSource _captchaImage = null;
        public System.Windows.Media.ImageSource CaptchaImage
        {
            get => _captchaImage;
            set => Set(ref _captchaImage, value);
        }
        #endregion

        #region Текст капчи
        private string _captchaText = string.Empty;
        public string CaptchaText
        {
            get => _captchaText;
            set => Set(ref _captchaText, value);
        }
        #endregion

        #region Текст сгенерированной капчи
        private string _generatedCaptchaText = string.Empty;
        public string GeneratedCaptchaText
        {
            get => _generatedCaptchaText;
            set => Set(ref _generatedCaptchaText, value);
        }
        #endregion

        #region Счетчик неправильных попыток
        private int _count = 0;
        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }
        #endregion

        private bool EntryClick(string Password)
        {
            bool result = false;
            List<Employees> Employees = (from em in Singleton.Instance.Context.Employees
                                         where em.login.ToLower() == LoginText.ToLower() &&
                                               em.password == Password
                                         select em).ToList();
            if (Employees.Count == 0)
            {
                MessageBox.Show("Пользователь не найден.");
                Count++;
            }
            else
            {
                result = true;
                Employees employee = Employees[0];
                Singleton.Instance.LoginedEmployee = employee;
                Singleton.Instance.LoadEmployeePage();
            }
            if (Count == 3)
            {
                CaptchaImageVisibility = Visibility.Visible;
                CaptchaTextBoxVisibility = Visibility.Visible;
                UpdateCaptchaTextBlockVisibility = Visibility.Visible;
            }
            return result;
        }

        private void UpdateImage(int Width = 250, int Height = 60)
        {
            CaptchaImage = BitmapToImageSource(CreateImage(Width, Height));
        }

        public System.Windows.Media.ImageSource BitmapToImageSource(Bitmap bmp)
        {
            try
            {
                IntPtr handle;
                handle = bmp.GetHbitmap();
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                return null;
            }
        }

        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();
            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);
            //Вычислим позицию текста
            int Xpos = 10;
            int Ypos = 10;
            //Добавим различные цвета ддя текста
            Brush[] colors = {
                Brushes.Black,
                Brushes.Red,
                Brushes.RoyalBlue,
                Brushes.Green,
                Brushes.Yellow,
                Brushes.White,
                Brushes.Tomato,
                Brushes.Sienna,
                Brushes.Pink };
            //Добавим различные цвета линий
            Pen[] colorpens = {
                Pens.Black,
                Pens.Red,
                Pens.RoyalBlue,
                Pens.Green,
                Pens.Yellow,
                Pens.White,
                Pens.Tomato,
                Pens.Sienna,
                Pens.Pink };
            //Делаем случайный стиль текста
            System.Drawing.FontStyle[] fontstyle = {
                System.Drawing.FontStyle.Bold,
                System.Drawing.FontStyle.Italic,
                System.Drawing.FontStyle.Regular,
                System.Drawing.FontStyle.Strikeout,
                System.Drawing.FontStyle.Underline};

            //Добавим различные углы поворота текста
            short[] rotate = { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5, 6, -6 };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage(result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Делаем случайный угол поворота текста
            g.RotateTransform(rnd.Next(rotate.Length));

            //Генерируем текст
            GeneratedCaptchaText = string.Empty;
            string symbols = string.Empty;
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            symbols += alphabet;
            symbols += alphabet.ToLower();
            symbols += "0123456789";
            for (int i = 0; i < 6; ++i)
            {
                GeneratedCaptchaText += symbols[rnd.Next(symbols.Length)];
            }

            //Нарисуем сгенирируемый текст
            g.DrawString(GeneratedCaptchaText,
                new Font("Arial", 25, fontstyle[rnd.Next(fontstyle.Length)]),
                colors[rnd.Next(colors.Length)],
                new PointF(Xpos, Ypos));

            //Добавим немного помех
            //Линии из углов
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
                new System.Drawing.Point(0, 0),
                new System.Drawing.Point(Width - 1, Height - 1));
            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
                new System.Drawing.Point(0, Height - 1),
                new System.Drawing.Point(Width - 1, 0));

            //Белые точки
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    if (rnd.Next() % 20 == 0)
                    {
                        result.SetPixel(i, j, Color.White);
                    }
                }
            }
            return result;
        }

        #region Команды
        public ICommand LoadEmployeesImagesCommand { get; }

        private bool CanLoadEmployeesImagesCommandExecute(object parameters) => true;

        private void OnLoadEmployeesImagesCommandExecuted(object parameters)
        {
            try
            {
                using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        uint findedEmployees = 0;
                        DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                        {
                            if (fileInfo.Extension == ".jpg")
                            {
                                string fileName = fileInfo.FullName;
                                string[] fullName = fileName.Substring(fileName.LastIndexOf('\\') + 1).Split('.')[0].Split(' ');
                                string surname = fullName[0];
                                string name = fullName[1];
                                string patronymic = fullName[2];
                                Employees employee = (from em in Singleton.Instance.Context.Employees
                                                      where em.name.ToLower() == name.ToLower() &&
                                                            em.surname.ToLower() == surname.ToLower() &&
                                                            em.patronymic.ToLower() == patronymic.ToLower()
                                                      select em).FirstOrDefault();
                                if (employee != null)
                                {
                                    findedEmployees++;
                                    employee.photo = Tools.GetImageBytes(fileInfo.FullName);
                                }
                            }
                        }
                        if (findedEmployees > 0)
                        {
                            Singleton.Instance.Context.SaveChanges();
                            MessageBox.Show($"Фото сотрудников загружено - {findedEmployees}.");
                        }
                        else
                        {
                            MessageBox.Show($"Фото сотрудников не найдены.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }

        public ICommand LoadMallImagesCommand { get; }

        private bool CanLoadMallImagesCommandExecute(object parameters) => true;

        private void OnLoadMallImagesCommandExecuted(object parameters)
        {
            try
            {
                using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                    {
                        uint findedMalls = 0;
                        DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                        {
                            if (fileInfo.Extension == ".jpg")
                            {
                                string fileName = fileInfo.FullName;
                                string mallName = fileName.Substring(fileName.LastIndexOf('\\') + 1).Split('.')[0];
                                Mall mall = (from m in Singleton.Instance.Context.Mall
                                             where m.mall_name == mallName
                                             select m).FirstOrDefault();
                                if (mall != null)
                                {
                                    findedMalls++;
                                    mall.photo = Tools.GetImageBytes(fileInfo.FullName);
                                }
                            }
                        }
                        if (findedMalls > 0)
                        {
                            Singleton.Instance.Context.SaveChanges();
                            MessageBox.Show($"Фото ТЦ загружено - {findedMalls}.");
                        }
                        else
                        {
                            MessageBox.Show($"Фото ТЦ не найдены.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
            }
        }

        public ICommand EntryCommand { get; }

        private bool CanEntryCommandExecute(object p) => true;

        private void OnEntryCommandExecuted(object p)
        {
            PasswordBox passwordBox = p as PasswordBox;
            string password = passwordBox.Password;
            if (Count != 3)
            {
                EntryClick(password);
            }
            else if (CaptchaText == GeneratedCaptchaText)
            {
                if (EntryClick(password))
                {
                    CaptchaImageVisibility = Visibility.Hidden;
                    CaptchaTextBoxVisibility = Visibility.Hidden;
                    UpdateCaptchaTextBlockVisibility = Visibility.Hidden;
                    Count = 0;
                    CaptchaText = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("Неверная капча.");
            }
            UpdateImage();
        }

        public ICommand UpdateCaptchaLinkCommand { get; }

        private bool CanUpdateCaptchaLinkCommandExecute(object p) => true;

        private void OnUpdateCaptchaLinkCommandExecuted(object p)
        {
            UpdateImage();
        }

        #endregion

        #region Конструктор
        public LoginPageViewModel()
        {
            LoadEmployeesImagesCommand = new LambdaCommand(OnLoadEmployeesImagesCommandExecuted, CanLoadEmployeesImagesCommandExecute);
            LoadMallImagesCommand = new LambdaCommand(OnLoadMallImagesCommandExecuted, CanLoadMallImagesCommandExecute);
            EntryCommand = new LambdaCommand(OnEntryCommandExecuted, CanEntryCommandExecute);
            UpdateCaptchaLinkCommand = new LambdaCommand(OnUpdateCaptchaLinkCommandExecuted, CanUpdateCaptchaLinkCommandExecute);
            UpdateImage();
        }
        #endregion
    }
}
