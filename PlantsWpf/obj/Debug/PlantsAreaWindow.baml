   M S B A M L   `   `   `  ����  	PlantsWpf     PlantsWpf.PlantsAreaWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35I EWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=nullF 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Add plants area��5   	   $��258.532��6!   $��334.936��62   .��5      � -   5         �Name$ RootGrid��5      $
��0,-1q�6   $
��Left=�6+   $	��313��6F    � ColumnDefinitions 5	   
   �� 5
      .+�5   
   �� -   5        $��OptimalTemperature��5   *   $��20��6    �� 	MaxLength$ 3��6   $
��Left=�6D   $��23��6_   $��	80,73,0,0q�5       �� TextWrapping$
 Wrap=�6%   $	��Top=�69   $��39��6Q   �� 5   
   -   5        $��OptimalHumidity��5   *   $��60��6   $ 3��6   $
��Left=�6A   $��23��6\   $��	80,98,0,0q�5      $
 Wrap=�6%   $	��Top=�69   $��39��6Q   �� 5   
   -   5        $��OptimalSoilPh��5   )   $��7��6   $ 3��6   $
��Left=�6>   $��23��6Y   $��
80,124,0,0q�6e   $
 Wrap=�5      $	��Top=�6&   $��39��6>   �� 5   
   -   5        $��OptimalNutrient��5   *   $��14��6   $ 3��6   $
��Left=�6A   $��23��6\   $��
80,149,0,0q�5      $
 Wrap=�6&   $	��Top=�6:   $��39��6R   �� 5   
   -   5        $��PlantNameBox��5      $
��Left=�6'   $��	80,16,0,0q�6B   $	��Top=�6U   $��95��6m   $��22��5      �� 5   
   $��Temperature��6   $
��Left=�6&   $��2,73,0,0q�6A   $	��Top=�6S   $��81��6k   $��26��5      $��0��5      �� 5   
   $��Nutrient��6   $
��Left=�6#   $��	2,148,0,0q�6>   $	��Top=�6Q   $��81��6i   $��24��5      $��0��5      �� 5   
   $��SoilPh��6   $
��Left=�6!   $��	4,124,0,0q�6<   $	��Top=�6O   $��81��6g   $��26��5      $��0��5      �� 5   
   $��Humidity��6   $
��Left=�6#   $��2,98,0,0q�6>   $	��Top=�6P   $��81��6h   $��26��5       $��0��5!      �� 5"   
   -   5        $��MinTemperature��5"   *   $��10��6   $ 3��6   $
��Left=�6@   $��23��6[   $��
120,73,0,0q�5#      $
 Wrap=�6&   $	��Top=�6:   $��40��6R   �� 5$   
   -   5        $��MinHumidity��5$   *   $��50��6   $ 3��6   $
��Left=�6=   $��23��6X   $��
120,98,0,0q�6d   $
 Wrap=�5%      $	��Top=�6&   $��40��6>   �� 5&   
   -	   5        $��	MinSoilPh��5&   )   $��5��6   $ 3��6   $
��Left=�6:   $��23��6U   $��120,124,0,0q�6a   $
 Wrap=�5'      $	��Top=�6&   $��40��6>   �� 5(   
   -
   5        $��MinNutrient��5(   )   $��9��6   $ 3��6   $
��Left=�6<   $��23��6W   $��120,149,0,0q�6c   $
 Wrap=�5)      $	��Top=�6&   $��40��6>   �� 5*   
   -   5        $��MaxTemperature��5*   *   $��30��6   $ 3��6   $
��Left=�6@   $��23��6[   $��
161,73,0,0q�5+      $
 Wrap=�6&   $	��Top=�6:   $��39��6R   �� 5,   
   -   5        $��MaxHumidity��5,   *   $��70��6   $ 3��6   $
��Left=�6=   $��23��6X   $��
161,98,0,0q�6d   $
 Wrap=�5-      $	��Top=�6&   $��39��6>   �� 5.   
   -   5        $��	MaxSoilPh��5.   )   $��9��6   $ 3��6   $
��Left=�6:   $��23��6U   $��161,124,0,0q�6a   $
 Wrap=�5/      $	��Top=�6&   $��39��6>   �� 50   
   -   5        $��MaxNutrient��50   +   $	��100��6   $ 3��6   $
��Left=�6>   $��23��6Y   $��161,149,0,0q�51      $
 Wrap=�6'   $	��Top=�6;   $��39��6S   �� 52   
   $��Optimal��6   $
��Left=�6"   $��	76,48,0,0q�6=   $	��Top=�6P   $��26��6h   $��52��53      �� 54   
   $	��Min��6   $
��Left=�6   $��
126,48,0,0q�69   $	��Top=�6M    J� RenderTransformOrigin$ 1.693,0.474&�55      $��26��64   $��30��6@   �� 56   
   $	��Max��6   $
��Left=�6   $��
161,48,0,0q�69   $	��Top=�6M   $��26��6e   $��32��57      �� 58   
   $��Number��6   $
��Left=�6!   $��
188,14,0,0q�6<   $	��Top=�6P   $��56��6h   $��26��59      $ 0.976,0.577&�6   �� 5:   
   -   5        $��Number��5:       $ 4��6   $��10��6.   $
��Left=�68   $��23��6S   $��
245,16,0,0q�6_   $
 Wrap=�5;      $	��Top=�6&   $��57��6>   �� 5<   
   -   5        $��	PlantName��5<       $��0��6   $��
Plant name��61   $
��Left=�6F   $��5,12,0,0q�6a   $	��Top=�5=      $��70��6(   $��26��63   $ 0.976,0.577&�6?   �� 5>   
   -   5        $
��Save��5>   !   $��0��6   $
��Save��6-   $
��Left=�6Q   $��29��6l   $��	4,182,0,0q�5?      $	��Top=�6$   $��64��6<   �� 5@   
   -   5        $��ClearAll��5@      $��	Clear all��6!   $
��Left=�6N   $��29��6i   $��
71,182,0,0q�5A      $	��Top=�6%   $��64��6=   $��0��5B      �� 5C   
   $��Is on��6   $
��Left=�6    $��
205,49,0,0q�6;   $	��Top=�6O   $ 1.737,-8.769&�5D      $��26��65   $��46��6A   �� 5E   
   -   5        $��TemperatureCheckBox��5E   $    d� 	IsChecked$
 TrueY�6   $
��Left=�6?   $��
213,76,0,0q�6Z   $	��Top=�5F      $��19��6+   $��21��67   �� 5G   
   -   5        $��HumidityCheckBox��5G   $   $
 TrueY�6   $
��Left=�6<   $��213,102,0,0q�6W   $	��Top=�5H      $��18��6+   $��21��67   �� 5I   
   -   5        $��SoilPhCheckBox��5I   $   $
 TrueY�6   $
��Left=�6:   $��213,128,0,0q�6U   $	��Top=�5J      $��17��6+   $��21��67   �� 5K   
   -   5        $��NutrientCheckBox��5K   $   $
 TrueY�6   $
��Left=�6<   $��213,153,0,0q�6W   $	��Top=�5L      $��18��6+   $��21��67   �� 5M   
   $��Timeout��6   $
��Left=�6"   $��
246,50,0,0q�6=   $	��Top=�6Q   $��26��6i   $��54��5N      �� 5O   
   -   5        $��TemperatureTimeout��5O      $
��Left=�6,   $��23��6G   $��
246,71,0,0q�6S   $
 Wrap=�5P      $	��Top=�6&   $��58��6>   $��00:00:01��6I   �� 5Q   
   -   5        $��HumidityTimeout��5Q      $
��Left=�6)   $��23��6D   $��
246,97,0,0q�6P   $
 Wrap=�6d   $	��Top=�5R      $��58��6*   $��00:00:01��65   �� 5S   
   -   5        $��SoilPhTimeout��5S      $
��Left=�6'   $��23��6B   $��246,123,0,0q�6N   $
 Wrap=�6c   $	��Top=�5T      $��58��6*   $��00:00:01��65   �� 5U   
   -   5        $��NutrientTimeout��5U      $
��Left=�6)   $��23��6D   $��246,149,0,0q�6P   $
 Wrap=�6e   $	��Top=�5V      $��58��6*   $��00:00:01��65   5W      5X      