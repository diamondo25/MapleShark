Update by KururuLABO
-Mapleshark script dialog interface
-Save script and refresh structure without close form
-Export script function
-Mapleshark script menu button
	-Import button (Ctrl+I)
	-Export button (Ctrl+E)
	-Save button(Ctrl+S)
	-Exit button(Ctrl+X)
-All structure still have value suffix
	/*Example 
	* AddInt("ID");
	* On structure panel
	* Before : ID
	* ID
	* After  : ID : Variable value 
	* ID : 251413 
	*/
-Add more functions for script (useful for variable)
	//Get value without given name to structure
	byte AddByte()
	sbyte AddSByte()
	ushort AddUShort()
	short AddShort()
	uint AddUInt()
	int AddInt()
	float AddFloat()
	double AddDouble()
	bool AddBool()
	long AddLong()
	string AddPaddedString(int length)
	void AddField(int length)

	//If use varible and function without given name to structure
	//Just use AddComment With length
	/*Example
	* job = AddShort();
	* AddComment("JOB : "+job,2); // short = 2
	*/ 
	void AddComment(string pComment,int pLength)


	void StartNodeWithVariable(string name,int length)
	//Start node with missing highlight at dataform

	/*Example
	* a = AddByte(); //get value without name
	* StartNodeWithVariable("Test "+a,1)
	* EndNode(false);
	* --------or------------
	* id = AddInt();
	* name = AddPaddedString(13); //get name [name length = 13]
	* StartNodeWithVariable("ID : "+id+" Name : "+name,17) //Highlight dataform with 17 byte for variable [id = 4, name = 13 | 13+4 = 17]
	* EndNode(false);
	*/

__________________________________________________________________________
Below update by Diamondo25

Interface
=========

Reserved.

Scripting
=========

The script engine used is called ScriptDotNet, also known as S#.
Information regarding the syntax of S# itself can be found at http://www.protsyk.com/scriptdotnet/

Functions defined below follow a simple syntax: <return type> <function name>(<parameters>)

Scripts have only one object exposed to them named ScriptAPI.
All of the following functions are called from the context of ScriptAPI, IE: ScriptAPI.AddByte("Example");


byte AddByte(string name)

  Adds unsigned byte as a field with given name to the structure view, and returns the value.
  
sbyte AddSByte(string name)

  Adds signed byte as a field with given name to the structure view, and returns the value.

ushort AddUShort(string name)

  Adds unsigned short as a field with given name to the structure view, and returns the value.

short AddShort(string name)

  Adds signed short as a field with given name to the structure view, and returns the value.

uint AddUInt(string name)

  Adds unsigned int as a field with given name to the structure view, and returns the value.

int AddInt(string name)

  Adds signed int as a field with given name to the structure view, and returns the value.

float AddFloat(string name)

  Adds 4 byte float as a field with given name to the structure view, and returns the value.

double AddDouble(string name)

  Adds 4 byte double as a field with given name to the structure view, and returns the value.

bool AddBool(string name)

  Adds 1 byte bool as a field with given name to the structure view, and returns the value (false when byte is 0, true when byte is 1).

long AddLong(string name)

  Adds signed long as a field with given name to the structure view, and returns the value.

double AddDouble(string name)

  Adds 8 byte double float as a field with given name to the structure view, and returns the value.

string AddPaddedString(string name, int length)

  Adds fixed length string as a field with given name and length to the structure view, and returns the value.

void AddField(string name, int length)

  Adds a field with given name and length to the structure view, and returns nothing.

void StartNode(string name)

  Adds a sub node with given name as the new parent until required matching EndNode, and returns nothing.

void EndNode(bool expand)

  Completes the last StartNode, expanding contents if expand is true, and returns nothing.

void Write(string file, string line)

  Appends the given line of text to the given file, and returns nothing.
  
int Remaining()

  Returns the number of bytes remaining unprocessed in the packet.



