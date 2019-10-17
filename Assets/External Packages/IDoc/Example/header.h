/// <summary> summary of the header file </summary>

/// <summary> summary of the macro  </summary>
#define MY_MACRO 10

/// <summary> summary of the struct </summary>
typedef struct ExampleStruct {
	/// <summary> summary of the field </summary>
	int example_field;
} ExampleStruct;

/// <summary> summary of the enum </summary>
typedef enum ExampleEnum {
	/// <summary> summary of the field A</summary>
	A, 
	/// <summary> summary of the field B</summary>
	B,
	/// <summary> summary of the field C</summary>
	C
}


/// <summary>
/// summary of the function1 
///	</summary>
/// <returns>the return val of the function1</returns>
int function1();

/// <summary>
/// summary of the function2 
///	</summary>
/// <param name="param1">the param 1</param>
void function2(char param1);

/// <summary>
/// summary of the function3 
///	</summary>
/// <param name="param1">the param 1</param>
/// <param name="param2">the param 2</param>
void function3(const int* param1, void (*param2)(int, char*));