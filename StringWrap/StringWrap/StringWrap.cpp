#include <iostream>
#include <string>

using namespace std;

class StringWrapper
{
public:
	static bool isWrapped(string string1, string string2)
	{
		int string1Length = string1.length();
		int string2Length = string2.length();

		if (string1Length != string2Length)
		{
			return false;
		}

		for (int i = 0; i < string1Length; ++i)
		{
			// Put the first letter to the back.
			string2 = string2.substr(1) + string2.substr(0, 1);

			// Then check if the strings are equal.
			if (string1.compare(string2) == 0)
			{
				return true;
			}
		}
		return false;
	}
};


static void printIsWrapped(string string1, string string2)
{
	StringWrapper stringWrapper;

	bool isWrapped = stringWrapper.isWrapped(string1, string2);

	cout << string1 << " and " << string2;
	if (!isWrapped)
	{
		cout << " don't ";
	}
	cout << " wrap.\n";
}

int main()
{
	string wrappedString = "WrappedString";
	string wrappedString2 = "ingWrappedStr";

	string notWrappedString = "testing";
	string notWrappedString2 = "tstinge";

	printIsWrapped(wrappedString, wrappedString2);
	printIsWrapped(notWrappedString, notWrappedString2);
	
	/*
	Output
	WrappedString and ingWrappedStr wrap.
	testing and tstinge don't wrap.
	*/

	return 0;
}
