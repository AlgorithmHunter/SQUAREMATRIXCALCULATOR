
using MauiApp4.Code;
using System;
using Color = Microsoft.Maui.Graphics.Color;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

using Microsoft.Maui.Controls.Shapes;
#if WINDOWS
using CommunityToolkit.Mvvm.Messaging;
using Code;
using Microsoft.Maui.Storage;
#endif

//

namespace MauiApp4;

public partial class MainPage : ContentPage
{
    int count = 0;
    static double deviceWidth = 0;


    bool firstResize = false;
    private bool _IsFlayoutOpen;

    public MainPage()
    {
        InitializeComponent();

        IsFlayoutOpen = true;
#if WINDOWS
        this.SizeChanged += MainPage_SizeChanged;
        this.Loaded += MainPage_Loaded;

    
        WeakReferenceMessenger.Default.Register<DetailPageStatus>(this, (r, m) =>
        {
            var res = m.Value;
            if (res.Equals("Maximized"))
            {
                if (res != null)
                {
                             
              Adjustlayout();

                }
              
            }
            else if (res.Equals("Restored"))
            {

            
                Adjustlayout();
              
            }
         
        });
#endif

    }



    private void MainPage_Loaded(object sender, EventArgs e)
    {

        Adjustlayout();
    }

    private void MainPage_SizeChanged(object sender, EventArgs e)
    {

        Adjustlayout();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();



        Adjustlayout();
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

    }

    void Adjustlayout()
    {
        if (((MatrixA.Width * 2) + 60) > this.Window.Width)
        {

            Grid.SetColumnSpan(MatrixA, 2);
            Grid.SetColumnSpan(MatrixB, 2);
            Grid.SetColumn(MatrixA, 0);
            Grid.SetColumn(MatrixB, 0);
            Grid.SetRow(MatrixA, 0);
            Grid.SetRow(MatrixB, 1);

        }
        else
        {



            Grid.SetColumnSpan(MatrixA, 1);
            Grid.SetColumnSpan(MatrixB, 1);
            Grid.SetColumn(MatrixA, 0);
            Grid.SetColumn(MatrixB, 1);
            Grid.SetRow(MatrixA, 0);
            Grid.SetRow(MatrixB, 0);

        }


    }
    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        deviceWidth = width;

        if (firstResize == false)
        {
            var defaultStepSize = 4;
#if WINDOWS
              var defaultsize = await SecureStorage.GetAsync("matrix_default_size");

              if (defaultsize != null)
              {
                  defaultStepSize=Convert.ToInt32( defaultsize);

              }
              else
              {

             await SecureStorage.Default.SetAsync("matrix_default_size", "4");
            }
#endif
            StepperA.Value = defaultStepSize;
            firstResize = true;

        }
        Adjustlayout();
    }
    public struct MatriceHolder
    {
        public Grid gr1;
        public object indices;
    }
    MatriceHolder initializeMatrix(int count, Color _col)
    {

        Grid Matrix1 = new Grid();
        Matrix1.Margin = new Thickness(2);
        Matrixindice[,] matrixp = new Matrixindice[count, count];
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                matrixp[i, j] = new Matrixindice();
                matrixp[i, j].Input.WidthRequest = 60;
                AttachedNumericValidationBehavior.SetAttachBehavior(matrixp[i, j].Input, true);
                matrixp[i, j].Input.HorizontalTextAlignment = TextAlignment.Center;

                matrixp[i, j].Input.BackgroundColor = _col;

            }
        }

        for (int i = 0; i < count; i++)
        {
            Matrix1.AddColumnDefinition(new ColumnDefinition() { Width = 65 });
            Matrix1.AddRowDefinition(new RowDefinition() { Height = 40 });

        }
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                Matrix1.SetColumn(matrixp[i, j].Input, j);
                Matrix1.SetRow(matrixp[i, j].Input, i);

                Matrix1.Children.Add(matrixp[i, j].Input);
            }
        }
        MatriceHolder mh = new MatriceHolder();
        mh.gr1 = Matrix1;
        mh.indices = matrixp;
        return mh;
        ;
    }
    Matrixindice[,] MultiplyMatrix(Matrixindice[,] matrix1a, Matrixindice[,] matrix2a)
    {
        int size = matrix1a.GetLength(0);
        Matrixindice[,] newMatrix = new Matrixindice[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                newMatrix[i, j] = new Matrixindice();
            }
        }

        for (int i = 0; i < size; i++)
        {
            for (

                int c = 0; c < size; c++)
            {
                int Total = 0;
                for (int j = 0; j < size; j++)
                {
                    var d1 = 0;
                    var d2 = 0;
                    int.TryParse(matrix1a[i, j].Input.Text, out d1);
                    int.TryParse(matrix2a[j, c].Input.Text, out d2);
                    Total += Convert.ToInt32(d1) * Convert.ToInt32(d2);

                }
                newMatrix[i, c].Input.Text = Total.ToString();
            }
        }

        return newMatrix;
    }

    void displayMatrixResults(Matrixindice[,] matrixp)
    {
        Grid Matrix1 = new Grid();
        int count = matrixp.GetLength(0);

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {

                matrixp[i, j].Input.WidthRequest = 60;
                matrixp[i, j].Input.HorizontalTextAlignment = TextAlignment.Center;

                matrixp[i, j].Input.BackgroundColor = Color.FromRgba(0, 1, 0, 0.1);

            }
            Matrix1.AddColumnDefinition(new ColumnDefinition() { Width = 65 });
            Matrix1.AddRowDefinition(new RowDefinition() { Height = 40 });

        }


        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                Matrix1.SetColumn(matrixp[i, j].Input, j);
                Matrix1.SetRow(matrixp[i, j].Input, i);

                Matrix1.Children.Add(matrixp[i, j].Input);
            }
        }
        MatrixC.IsVisible = true;
        MatrixC.Children.Clear();

        MatrixC.Children.Add(Matrix1);

    }



    private async void Button_Clicked(object sender, EventArgs e)
    {


        var mat1 = (Matrixindice[,])aMatrix.indices;
        var mat2 = (Matrixindice[,])bMatrix.indices;
        if (mat1 != null)
        {
            var c = MultiplyMatrix(mat1, mat2);

            displayMatrixResults(c);

        }
        else
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var toast = Toast.Make("Create matrice size before calculating", ToastDuration.Short, 14);

            await toast.Show(cancellationTokenSource.Token);
        }

    }

    MatriceHolder aMatrix;
    MatriceHolder bMatrix;

    public bool IsFlayoutOpen { get => _IsFlayoutOpen; set => _IsFlayoutOpen = value; }
    private async void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {


        MatrixA.IsVisible = true;
        MatrixB.IsVisible = true;
        MatrixC.IsVisible = false;
        aMatrix = initializeMatrix(Convert.ToInt32(e.NewValue), Color.FromRgba(0, 0, 0, 0.1));
        bMatrix = initializeMatrix(Convert.ToInt32(e.NewValue), Color.FromRgba(0, 0, 1, 0.2));
        MatrixA.Children.Clear();
        MatrixB.Children.Clear();
        MatrixA.Children.Add(aMatrix.gr1);
        MatrixB.Children.Add(bMatrix.gr1);


#if WINDOWS
        await SecureStorage.Default.SetAsync("matrix_default_size", e.NewValue.ToString());
#endif
        Adjustlayout();


    }




}

