using System.Collections.Generic;

public class Stack<T> {
    private List<T> stackList;
    public Stack() {
        stackList = new();
    }
    public Stack(T element) {
        stackList = new() { element };
    }
    public Stack(List<T> elements) {
        stackList = elements;
    }
    public void Push(T element) {
        stackList.Insert(0, element);
    }
    public T Pop() {
        if (stackList.Count == 0) {
            return default;
        }
        T frontElement = stackList[0];
        stackList.RemoveAt(0);
        return frontElement;
    }
    public void Clear() {
        stackList.Clear();
    }
    public T Peek() {
        if (stackList.Count == 0) {
            return default;
        }
        return stackList[0];
    }
    public int Size() {
        return stackList.Count;
    }
    public bool IsEmpty() {
        return stackList.Count == 0;
    }
}
