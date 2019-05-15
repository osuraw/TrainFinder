﻿using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Desktop
{
    public static class PageAnimations
    {
        public static async Task SlideAndFadeInFromRight(this Page page,float seconds)
        {
           var sb = new Storyboard();
           sb.AddSliderFromRight(seconds,page.WindowWidth);
           sb.AddFadeIn(seconds);
           sb.Begin(page);
           page.Visibility = Visibility.Visible;
           await Task.Delay((int) seconds * 1000);
        }

        public static async Task SlideAndFadeOutToLeft(this Page page, float seconds)
        {
            var sb = new Storyboard();
            sb.AddSliderToLeft(seconds, page.WindowWidth);
            sb.AddFadeOut(seconds);
            sb.Begin(page);
            page.Visibility = Visibility.Visible;
            await Task.Delay((int)seconds * 1000);
        }
    }
}
