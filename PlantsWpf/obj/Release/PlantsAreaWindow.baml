   M S B A M L   `   `   `  ����   	PlantsWpf     PlantsWpf.PlantsAreaWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35I EWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=nullF 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  Add plants area��$��258.532��$��334.936��.��� -    �Name$ RootGrid��$
��0,-1q�$
��Left=�$	��313�� � ColumnDefinitions �� .+��� -   $��OptimalTemperature��$��20�� �� 	MaxLength$ 3��$
��Left=�$��23��$��	80,73,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��39���� -   $��OptimalHumidity��$��60��$ 3��$
��Left=�$��23��$��	80,98,0,0q�$
 Wrap=�$	��Top=�$��39���� -   $��OptimalSoilPh��$��7��$ 3��$
��Left=�$��23��$��
80,124,0,0q�$
 Wrap=�$	��Top=�$��39���� -   $��OptimalNutrient��$��14��$ 3��$
��Left=�$��23��$��
80,149,0,0q�$
 Wrap=�$	��Top=�$��39���� -   $��PlantNameBox��$
��Left=�$��	80,16,0,0q�$	��Top=�$��95��$��22���� $��Temperature��$
��Left=�$��2,73,0,0q�$	��Top=�$��81��$��26��$��0���� $��Nutrient��$
��Left=�$��	2,148,0,0q�$	��Top=�$��81��$��24��$��0���� $��SoilPh��$
��Left=�$��	4,124,0,0q�$	��Top=�$��81��$��26��$��0���� $��Humidity��$
��Left=�$��2,98,0,0q�$	��Top=�$��81��$��26��$��0���� -   $��MinTemperature��$��10��$ 3��$
��Left=�$��23��$��
120,73,0,0q�$
 Wrap=�$	��Top=�$��40���� -   $��MinHumidity��$��50��$ 3��$
��Left=�$��23��$��
120,98,0,0q�$
 Wrap=�$	��Top=�$��40���� -	   $��	MinSoilPh��$��5��$ 3��$
��Left=�$��23��$��120,124,0,0q�$
 Wrap=�$	��Top=�$��40���� -
   $��MinNutrient��$��9��$ 3��$
��Left=�$��23��$��120,149,0,0q�$
 Wrap=�$	��Top=�$��40���� -   $��MaxTemperature��$��30��$ 3��$
��Left=�$��23��$��
161,73,0,0q�$
 Wrap=�$	��Top=�$��39���� -   $��MaxHumidity��$��70��$ 3��$
��Left=�$��23��$��
161,98,0,0q�$
 Wrap=�$	��Top=�$��39���� -   $��	MaxSoilPh��$��9��$ 3��$
��Left=�$��23��$��161,124,0,0q�$
 Wrap=�$	��Top=�$��39���� -   $��MaxNutrient��$	��100��$ 3��$
��Left=�$��23��$��161,149,0,0q�$
 Wrap=�$	��Top=�$��39���� $��Optimal��$
��Left=�$��	76,48,0,0q�$	��Top=�$��26��$��52���� $	��Min��$
��Left=�$��
126,48,0,0q�$	��Top=� J� RenderTransformOrigin$ 1.693,0.474&�$��26��$��30���� $	��Max��$
��Left=�$��
161,48,0,0q�$	��Top=�$��26��$��32���� $��Number��$
��Left=�$��
188,14,0,0q�$	��Top=�$��56��$��26��$ 0.976,0.577&��� -   $��Number��$ 4��$��10��$
��Left=�$��23��$��
245,16,0,0q�$
 Wrap=�$	��Top=�$��57���� -   $��	PlantName��$��0��$��
Plant name��$
��Left=�$��5,12,0,0q�$	��Top=�$��70��$��26��$ 0.976,0.577&��� -   $
��Save��$��0��$
��Save��$
��Left=�$��29��$��	4,182,0,0q�$	��Top=�$��64���� -   $��ClearAll��$��	Clear all��$
��Left=�$��29��$��
71,182,0,0q�$	��Top=�$��64��$��0���� $��Is on��$
��Left=�$��
205,49,0,0q�$	��Top=�$ 1.737,-8.769&�$��26��$��46���� -   $��TemperatureCheckBox�� d� 	IsChecked$
 TrueY�$
��Left=�$��
213,76,0,0q�$	��Top=�$��19��$��21���� -   $��HumidityCheckBox��$
 TrueY�$
��Left=�$��213,102,0,0q�$	��Top=�$��18��$��21���� -   $��SoilPhCheckBox��$
 TrueY�$
��Left=�$��213,128,0,0q�$	��Top=�$��17��$��21���� -   $��NutrientCheckBox��$
 TrueY�$
��Left=�$��213,153,0,0q�$	��Top=�$��18��$��21���� $��Timeout��$
��Left=�$��
246,50,0,0q�$	��Top=�$��26��$��54���� -   $��TemperatureTimeout��$
��Left=�$��23��$��
246,71,0,0q�$
 Wrap=�$	��Top=�$��58��$��00:00:01���� -   $��HumidityTimeout��$
��Left=�$��23��$��
246,97,0,0q�$
 Wrap=�$	��Top=�$��58��$��00:00:01���� -   $��SoilPhTimeout��$
��Left=�$��23��$��246,123,0,0q�$
 Wrap=�$	��Top=�$��58��$��00:00:01���� -   $��NutrientTimeout��$
��Left=�$��23��$��246,149,0,0q�$
 Wrap=�$	��Top=�$��58��$��00:00:01��