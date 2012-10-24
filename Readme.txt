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

