using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Desktop
{
    public static  class StoryBoardHelpers
    {
        public static void  AddSliderFromRight(this Storyboard storyboard,float seconds, double offset, double decelerationRatio = 0.9f)
        {
            var slideAnimation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(offset, 0, -offset, 0),
                To = new Thickness(0),
                DecelerationRatio = 0.9f,
            };
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));

           storyboard.Children.Add(slideAnimation);
            
        }
        public static void AddSliderToLeft(this Storyboard storyboard, float seconds, double offset, double decelerationRatio = 0.9f)
        {
            var slideAnimation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(-offset, 0, offset, 0),
                DecelerationRatio = 0.9f,
            };
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));

            storyboard.Children.Add(slideAnimation);

        }
        public static void  AddFadeIn(this Storyboard storyboard,float seconds)
        {
            var animation = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

           storyboard.Children.Add(animation);
        }
        public static void AddFadeOut(this Storyboard storyboard, float seconds)
        {
            var animation = new DoubleAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            storyboard.Children.Add(animation);
        }
    }
}