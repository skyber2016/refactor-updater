using System;
using System.Collections.Generic;

namespace Update.Classes.Functions;

internal class BandwidthList<T> : List<T>
{
	private int index;

	private int maxCapacity { get; set; }

	public BandwidthList(int capacity)
	{
		maxCapacity = capacity;
	}

	public new void Add(T newElement)
	{
		if (index + 1 == maxCapacity)
		{
			index = 0;
		}
		Insert(index, newElement);
		index++;
	}

	public double CalculateAverage()
	{
		double num = 0.0;
		for (int i = 0; i < base.Count; i++)
		{
			try
			{
				num += Convert.ToDouble(base[i]);
			}
			catch (Exception)
			{
			}
		}
		return num / (double)base.Count;
	}
}
