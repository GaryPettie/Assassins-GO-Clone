using UnityEngine;

///<summary>The summary of the namespace 'TestNamespace'</summary>
namespace TestNamespace {
	
	///<summary>The summary of the class 'MyCSClass'</summary>
	public class TestClass  {
		
		///<summary>This is a field comment</summary>
		[SerializeField]
		public string field;

		///<summary>This is a property comment</summary>
		///<value>This a property value comment</value>
		public int Property {get; set;}
		
		///<summary>
		/// This is a Test function which changes the value of <see cref="field"/> to
		/// The value of the param <paramref name="arg"/>.
		///</summary>
        ///<param name="arg">Function parameter comment</param>
		///<remarks>This is a remarks</remarks>
		///<example>
		///	The following code is an example :
		/// <code>
		/// using TestNamespace;
		///
		///	public class TestClass {
		///
		///		void Test()
		///		{
		///			var test = new MyCSClass();
		///			test.MyFunction("TEST STRING");
		///			Console.WriteLine(test.field);
		///		}	
		/// }
		///</code>
		///</example>
		public string Function(string arg){
			return null;
		}
	}

	public enum TestEnum
	{
		///<summary>A Comment</summary>
		A,
		///<summary>B Comment</summary>
		B,
		///<summary>C Comment</summary>
		C
	}
}