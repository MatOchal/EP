﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D; 
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3DGraphicsEditor
{

    public partial class MainWindow : Window
    {
        //Model3DGroup shape;
        List<ModelVisual3D> shapes;
        List<string> shapesNames;

        private PerspectiveCamera camera;
        private double camera_PosX = 9;
        private double camera_PosY = 8;
        private double camera_PosZ = 10;
        private double camera_DirX = -9;
        private double camera_DirY = -8;
        private double camera_DirZ = -10;


        public MainWindow()
        {
            InitializeComponent();
            shapes = new List<ModelVisual3D>();
            shapesNames = new List<string>();
            syncShapeNames();
        }
        private int cuboidcount = 0;
        private int prismcount = 0;

        private void addShapeButtonClick(object sender, RoutedEventArgs e)
        {
            double xPos = Convert.ToDouble(shapePositionXTextBox.Text);
            double yPos = Convert.ToDouble(shapePositionYTextBox.Text);
            double zPos = Convert.ToDouble(shapePositionZTextBox.Text);

            ListBoxItem s = (ListBoxItem)addShapelistBox.SelectedItem;

            if (addShapelistBox.SelectedItem.Equals(null) == true)
            {
                //error
            }
            else
            {
                if (s.Content.ToString() == "Cuboid")
                {//add cube
                    AddCube(xPos, yPos, zPos, 1, 1, 1);
                    cuboidcount += 1;
                }

                if (s.Content.ToString() == "Prism")
                {// add triange
                    AddPrism(xPos, yPos, zPos, 1, 0.7, 1);
                    prismcount += 1;
                }

                updateViewportListBox();
            }
        }

        private void AddCube(double x_offset, double y_offset, double z_offset, double x_size, double y_size, double z_size)
        {
            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(x_size, 0, 0);
            Point3D p2 = new Point3D(x_size, 0, z_size);
            Point3D p3 = new Point3D(0, 0, z_size);
            Point3D p4 = new Point3D(0, y_size, 0);
            Point3D p5 = new Point3D(x_size, y_size, 0);
            Point3D p6 = new Point3D(x_size, y_size, z_size);
            Point3D p7 = new Point3D(0, y_size, z_size);

            Model3DGroup shape;
            shape = new Model3DGroup();
            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.SkyBlue));
            //front side triangles
            shape.Children.Add(CreateTriangleModel(p3, p2, p6, material));
            shape.Children.Add(CreateTriangleModel(p3, p6, p7, material));
            //right side triangles
            shape.Children.Add(CreateTriangleModel(p2, p1, p5, material));
            shape.Children.Add(CreateTriangleModel(p2, p5, p6, material));
            //back side triangles
            shape.Children.Add(CreateTriangleModel(p1, p0, p4, material));
            shape.Children.Add(CreateTriangleModel(p1, p4, p5, material));
            //left side triangles
            shape.Children.Add(CreateTriangleModel(p0, p3, p7, material));
            shape.Children.Add(CreateTriangleModel(p0, p7, p4, material));
            //top side triangles
            shape.Children.Add(CreateTriangleModel(p7, p6, p5, material));
            shape.Children.Add(CreateTriangleModel(p7, p5, p4, material));
            //bottom side triangles
            shape.Children.Add(CreateTriangleModel(p2, p3, p0, material));
            shape.Children.Add(CreateTriangleModel(p2, p0, p1, material));

            ModelVisual3D model = new ModelVisual3D();
            model.Content = shape;

            Transform3DGroup myTransform3DGroup = new Transform3DGroup();
            TranslateTransform3D myTransform3D = new TranslateTransform3D();
            myTransform3D.OffsetY = y_offset;
            myTransform3D.OffsetX = x_offset;
            myTransform3D.OffsetZ = z_offset;

            // Add the scale transform to the Transform3DGroup.
            myTransform3DGroup.Children.Add(myTransform3D);

            model.Transform = myTransform3DGroup;
            //shapes.Add(shape);
            this.mainViewport.Children.Add(model);
            shapes.Add(model);

            string str = "Cuboid " + cuboidcount.ToString();
            shapesNames.Add(str);

        }

        private void AddPrism(double x_offset, double y_offset, double z_offset, double x_size, double y_size, double z_size)
        {
            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(x_size, 0, 0);
            Point3D p2 = new Point3D(x_size, 0, z_size);
            Point3D p3 = new Point3D(0, 0, z_size);
            Point3D p4 = new Point3D(x_size / 2, y_size, z_size / 2);
            
            Model3DGroup shape;
            shape = new Model3DGroup();
            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.SkyBlue));

            //bottom side triangles
            shape.Children.Add(CreateTriangleModel(p2, p3, p0, material));
            shape.Children.Add(CreateTriangleModel(p2, p0, p1, material));
            
            //front side triangle
            shape.Children.Add(CreateTriangleModel(p4, p3, p2, material));

            //right side triangle
            shape.Children.Add(CreateTriangleModel(p4, p2, p1, material));
            
            //back side triangle
            shape.Children.Add(CreateTriangleModel(p4, p1, p0, material));
            
            //left side triangle
            shape.Children.Add(CreateTriangleModel(p4, p0, p3, material));

            ModelVisual3D model = new ModelVisual3D();
            model.Content = shape;

            Transform3DGroup myTransform3DGroup = new Transform3DGroup();
            TranslateTransform3D myTransform3D = new TranslateTransform3D();
            myTransform3D.OffsetY = y_offset;
            myTransform3D.OffsetX = x_offset;
            myTransform3D.OffsetZ = z_offset;

            // Add the scale transform to the Transform3DGroup.
            myTransform3DGroup.Children.Add(myTransform3D);

            model.Transform = myTransform3DGroup;
            //shapes.Add(shape);
            this.mainViewport.Children.Add(model);
            shapes.Add(model);

            string str = "Prism " + prismcount.ToString();
            shapesNames.Add(str);

        }


        private void syncShapeNames()
        {

            ModelVisual3D m;
            int i = 0;
            while (mainViewport.Children.Count - shapes.Count > 0)
            {
                m = (ModelVisual3D)mainViewport.Children[i];
                shapes.Add(m);
                if (m.Content is DirectionalLight == true) { shapesNames.Add("Light"); }
                else { shapesNames.Add("Some Shape"); }
                i++;
            }
        }

        private void updateViewportListBox()
        {
            int i;

            for (i = ViewportListBox.Items.Count - 1; i >= 0; i--)
            {
                ViewportListBox.Items.RemoveAt(i);
            }

            for (i = 0; i <= shapesNames.Count - 1; i++)
            {
                ViewportListBox.Items.Add(shapesNames[i]);
            }
        }

        private void doubleClickViewportListbox(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem s = (ListBoxItem)addShapelistBox.SelectedItem;
            int Itemindex = ViewportListBox.SelectedIndex;

            if (Itemindex == -1) {}
            else
            {
                selectedItem(Itemindex);
            }
        }

        private void selectedItem(int index)
        {
            /**
            Transform3DGroup myTransform3DGroup = new Transform3DGroup();
            ScaleTransform3D myScaleTransform3D = new ScaleTransform3D();
            myScaleTransform3D.ScaleX = 2;
            myScaleTransform3D.ScaleY = 0.5;
            myScaleTransform3D.ScaleZ = 1;
            myTransform3DGroup.Children.Add(myScaleTransform3D);

            ModelVisual3D m;
            for (int i = mainViewport.Children.Count - 1; i >= 0; i--)
            {
                m = (ModelVisual3D)mainViewport.Children[i];
                if (m == shapes[index])
                    m.Transform = myScaleTransform3D;
            }**/

            ModelVisual3D m;
            m = (ModelVisual3D)mainViewport.Children[index];
            if (m.Content is DirectionalLight == false)
            {
                mainViewport.Children.Remove(m);
                shapes.Remove(shapes[index]);
                shapesNames.Remove(shapesNames[index]);
            }
            updateViewportListBox();

        }

        private void updateListButtonClick(object sender, RoutedEventArgs e)
        {
            string str;

            ModelVisual3D m;
            for (int i = mainViewport.Children.Count - 1; i >= 0; i--)
            {
                m = (ModelVisual3D)mainViewport.Children[i];
                shapes.Add(m);
                if (m.Content is DirectionalLight == true) { str = "Light"; }
                else { str = "Shape"; }
                shapesNames.Add(str);
            }
            updateViewportListBox();
        }

        private void setBackground(double size, Material material)
        {
            Model3DGroup background = new Model3DGroup();
            Point3D p0 = new Point3D(-size, 0, -size);
            Point3D p1 = new Point3D(size, 0, -size);
            Point3D p2 = new Point3D(size, 0, size);
            Point3D p3 = new Point3D(-size, 0, size);

            background.Children.Add(CreateTriangleModel(p2, p1, p0, material));
            background.Children.Add(CreateTriangleModel(p0, p3, p2, material));

            ModelVisual3D model = new ModelVisual3D();
            model.Content = background;
            this.mainViewport.Children.Add(model);
            shapes.Add(model);
            shapesNames.Add("Background");
        }

        private Vector3D CalculateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(
                p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(
                p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }

        private void ClearViewport()
        {
            ModelVisual3D m;
            for (int i = mainViewport.Children.Count - 1; i >= 0; i--)
            {
                m = (ModelVisual3D)mainViewport.Children[i];
                if (m.Content is DirectionalLight == false)
                {
                    mainViewport.Children.Remove(m);
                    shapes.Remove(shapes[i]);
                    shapesNames.Remove(shapesNames[i]);
                }
            }
            updateViewportListBox();
        }


        //********************************************************************************************************************************
        private void simpleButtonClick(object sender, RoutedEventArgs e)
        {
            Model3DGroup triangle = new Model3DGroup();
            Point3D p0 = new Point3D(0, 1, 0);
            Point3D p1 = new Point3D(5, 1, 0);
            Point3D p2 = new Point3D(0, 1, 5);

            Point3D p3 = new Point3D(0, 10, 0);
            Point3D p4 = new Point3D(5, 10, 0);
            Point3D p5 = new Point3D(0, 10, 5);
            /**/

            ScaleTransform3D s = new ScaleTransform3D();
            s.CenterX = 1;
            s.CenterY = 1;
            s.CenterZ = 1;
            s.ScaleX = 2;
            s.ScaleY = 4;
            s.ScaleZ = 14;
            s.Transform(p0);
            s.Transform(p1);
            s.Transform(p2);

            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.DarkKhaki));
            triangle.Children.Add(CreateTriangleModel(p2, p1, p0, material));
            triangle.Children.Add(CreateTriangleModel(p5, p4, p3, material));

            ModelVisual3D model = new ModelVisual3D();
            model.Content = triangle;
            this.mainViewport.Children.Add(model);
        }



        private void cameraButtonClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void cubeButtonClick(object sender, RoutedEventArgs e)
        {
            Point3D p0 = new Point3D(0, 0, 0);
            Point3D p1 = new Point3D(5, 0, 0);
            Point3D p2 = new Point3D(5, 0, 5);
            Point3D p3 = new Point3D(0, 0, 5);
            Point3D p4 = new Point3D(0, 5, 0);
            Point3D p5 = new Point3D(5, 5, 0);
            Point3D p6 = new Point3D(5, 5, 5);
            Point3D p7 = new Point3D(0, 5, 5);

            Model3DGroup shape;
            shape = new Model3DGroup();
            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.SkyBlue));
            //front side triangles
            shape.Children.Add(CreateTriangleModel(p3, p2, p6, material));
            shape.Children.Add(CreateTriangleModel(p3, p6, p7, material));
            //right side triangles
            shape.Children.Add(CreateTriangleModel(p2, p1, p5, material));
            shape.Children.Add(CreateTriangleModel(p2, p5, p6, material));
            //back side triangles
            shape.Children.Add(CreateTriangleModel(p1, p0, p4, material));
            shape.Children.Add(CreateTriangleModel(p1, p4, p5, material));
            //left side triangles
            shape.Children.Add(CreateTriangleModel(p0, p3, p7, material));
            shape.Children.Add(CreateTriangleModel(p0, p7, p4, material));
            //top side triangles
            shape.Children.Add(CreateTriangleModel(p7, p6, p5, material));
            shape.Children.Add(CreateTriangleModel(p7, p5, p4, material));
            //bottom side triangles
            shape.Children.Add(CreateTriangleModel(p2, p3, p0, material));
            shape.Children.Add(CreateTriangleModel(p2, p0, p1, material));

            ModelVisual3D model = new ModelVisual3D();
            model.Content = shape;
            //shapes.Add(shape);
            this.mainViewport.Children.Add(model);
        }

        private Model3DGroup CreateTriangleModel(Point3D p0, Point3D p1, Point3D p2, Material material)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            Vector3D normal = CalculateNormal(p0, p1, p2);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }



        private void CreateBackground()
        {

        }


        private void SetCamera()
        {
            camera = (PerspectiveCamera)mainViewport.Camera;
            Point3D position = new Point3D(camera_PosX,camera_PosY,camera_PosZ);
            camera_DirX = -camera_PosX;
            camera_DirY = -camera_PosY;
            camera_DirZ = -camera_PosZ;
            Vector3D lookDirection = new Vector3D(camera_DirX,camera_DirY,camera_DirZ);

            camera.Position = position;
            camera.LookDirection = lookDirection;

            cameraPositionXTextBox.Text = Convert.ToString(camera_PosX);
            cameraPositionYTextBox.Text = Convert.ToString(camera_PosY);
            cameraPositionZTextBox.Text = Convert.ToString(camera_PosZ);


        }

        private void mainViewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double h;
            double ratio = 1;
            h = Math.Sqrt(camera_PosX * camera_PosX + camera_PosY * camera_PosY + camera_PosZ * camera_PosZ);
            
            if (e.Delta > 0)
            {
                ratio = h / (h + 1);
            }

            else if (e.Delta < 0)
            {
                ratio = (h + 1) / h;
            }

            camera_PosX *= ratio;
            camera_PosY *= ratio;
            camera_PosZ *= ratio;

            SetCamera();
        }

        private void mainViewport_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void StackPanel_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q) { camera_PosX -= 1; }
            if (e.Key == Key.W) { camera_PosX += 1; }
            if (e.Key == Key.A) { camera_PosY -= 1; }
            if (e.Key == Key.S) { camera_PosY += 1; }
            if (e.Key == Key.Z) { camera_PosZ -= 1; }
            if (e.Key == Key.X) { camera_PosZ += 1; }

            if ((e.Key == Key.E) || (e.Key == Key.R) || (e.Key == Key.F) || (e.Key == Key.D))
            {
                //rotate around y-axis
                double h = Math.Sqrt(camera_PosX*camera_PosX + camera_PosZ*camera_PosZ);
                double angle_theta = 0.1;
                double angle_alpha = Math.Atan(camera_PosX/camera_PosZ) - angle_theta;

                double angle_beta1 = ((Math.PI - angle_theta) / 2) - (angle_alpha + angle_theta);
                double angle_beta2 = ((Math.PI - angle_theta) / 2) - angle_alpha;

                double small_h = 2 * h * Math.Sin(angle_theta / 2);
                double delta_x = 1;
                double delta_z = 1;


                if ((camera_PosX < camera_PosZ) & (camera_PosX > 0)) 
                    { delta_x = -delta_x; }
                
                if ((camera_PosZ < -h) || (camera_PosZ > h)) { delta_z = -delta_z; }


                if (e.Key == Key.E)
                {
                    //delta_x = small_h * Math.Cos(angle_beta2);
                    //delta_z = -small_h * Math.Sin(angle_beta2);

                }

                if (e.Key == Key.R)
                {
                    //delta_x = -small_h * Math.Cos(angle_beta1);
                    //delta_z = small_h * Math.Sin(angle_beta1);
                }

                //if ((camera_PosX < -h) || (camera_PosX > h)) { delta_x = -delta_x; }
                //if ((camera_PosZ < -h) || (camera_PosZ > h)) { delta_z = -delta_z; }
                
                camera_PosX += delta_x;
                camera_PosZ += delta_z;



            }
            if (e.Key == Key.D1)
            {
                //create triange 
            }

            if (e.Key == Key.D2)
            {
                //create square 
            }

            if (e.Key == Key.D3)
            {
                //create cube
            }


            SetCamera();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void clearButtonClick(object sender, RoutedEventArgs e)
        {
            ClearViewport();
        }

        private void backgroundButtonClick(object sender, RoutedEventArgs e)
        {
            Material material = new DiffuseMaterial(new SolidColorBrush(Colors.Green));
            setBackground(20, material);
        }

        private void Transform()
        {
            Transform3DGroup myTransform3DGroup = new Transform3DGroup();

            // Create and apply a scale transformation that stretches the object along the local x-axis   
            // by 200 percent and shrinks it along the local y-axis by 50 percent.
            ScaleTransform3D myScaleTransform3D = new ScaleTransform3D();
            myScaleTransform3D.ScaleX = 2;
            myScaleTransform3D.ScaleY = 0.5;
            myScaleTransform3D.ScaleZ = 1;

            // Add the scale transform to the Transform3DGroup.
            myTransform3DGroup.Children.Add(myScaleTransform3D);

            // Set the Transform property of the GeometryModel to the Transform3DGroup which includes  
            // both transformations. The 3D object now has two Transformations applied to it.
            //mainViewport.c = myTransform3DGroup;

            Model3DGroup mg = new Model3DGroup();

            ModelVisual3D m;
            for (int i = mainViewport.Children.Count - 1; i >= 0; i--)
            {
                m = (ModelVisual3D)mainViewport.Children[i];
                if (m.Content is DirectionalLight == false)
                {
                    //if (m.Content == shape)
                    //{
                        m.Transform = myScaleTransform3D;
                    //}
                }

            }
        }

        private void scaleButtonClick(object sender, RoutedEventArgs e)
        {
            Transform();
        }


        int selectedShapeIndex;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // selection change
            ListBoxItem s = (ListBoxItem)addShapelistBox.SelectedItem;
            int Itemindex = ViewportListBox.SelectedIndex;
            selectedShapeIndex = Itemindex;
            if (Itemindex == -1) { }
            else
            {
                displayEditShapeMenu(Itemindex);
            }
        }

        private void displayEditShapeMenu(int itemIndex)
        {
            ModelVisual3D m;
            m = (ModelVisual3D)mainViewport.Children[itemIndex];
            if ((m.Content is DirectionalLight == false) & (shapesNames.IndexOf("background") != itemIndex))
            {
                {
                    //Model3DGroup mg = (Model3DGroup)m.Children[0];
                }
            }
        }

        private void editShapeButtonClick(object sender, RoutedEventArgs e)
        {
            double posX = Convert.ToDouble(EditShapePosXTextBox.Text);
            double posY = Convert.ToDouble(EditShapePosYTextBox.Text);
            double posZ = Convert.ToDouble(EditShapePosZTextBox.Text);
            double sizeX = Convert.ToDouble(EditShapeSizeXTextBox.Text);
            double sizeY = Convert.ToDouble(EditShapeSizeYTextBox.Text);
            double sizeZ = Convert.ToDouble(EditShapeSizeZTextBox.Text);

            Transform3DGroup myTransform3DGroup = new Transform3DGroup();
            ScaleTransform3D myScaleTransform3D = new ScaleTransform3D();
            myScaleTransform3D.ScaleX = sizeX;
            myScaleTransform3D.ScaleY = sizeY;
            myScaleTransform3D.ScaleZ = sizeZ;


            TranslateTransform3D myTranslateTransform3D = new TranslateTransform3D();
            myTranslateTransform3D.OffsetY = posY;
            myTranslateTransform3D.OffsetX = posX;
            myTranslateTransform3D.OffsetZ = posZ;

            // Add the scale transform to the Transform3DGroup.
            myTransform3DGroup.Children.Add(myScaleTransform3D);
            myTransform3DGroup.Children.Add(myTranslateTransform3D);

            ModelVisual3D m = (ModelVisual3D)mainViewport.Children[selectedShapeIndex];
            m.Transform = myTransform3DGroup;
            shapes[selectedShapeIndex] = m;


        }



        

    }
}