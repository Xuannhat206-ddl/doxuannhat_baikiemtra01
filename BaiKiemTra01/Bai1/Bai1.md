# BÀI TẬP C# PHẦN LÝ THUYẾT & CÂU HỎI NGẮN

## Câu 1: Phân biệt Value Types và Reference Types về cơ chế lưu trữ Stack và Heap

### 1. Khái niệm

* **Value Types (kiểu giá trị):** Lưu trữ giá trị trực tiếp. Khi gán biến, giá trị được sao chép độc lập. Ví dụ: `int`, `double`, `bool`, `struct`.
* **Reference Types (kiểu tham chiếu):** Biến lưu tham chiếu đến đối tượng. Khi gán biến, tham chiếu được sao chép nên nhiều biến có thể cùng tham chiếu một đối tượng. Ví dụ: `class`, `array`, `string`.

**Stack** thường lưu thông tin thực thi và biến cục bộ; **Heap** thường lưu các đối tượng được cấp phát động. Vị trí thực tế phụ thuộc vào ngữ cảnh, không phải mọi Value Type đều nằm trên Stack.

### 2. Ví dụ Value Types

```csharp
int a = 10;
int b = a;
b = 20;

Console.WriteLine(a); // 10
Console.WriteLine(b); // 20
```

**Giải thích:** `b` nhận bản sao giá trị của `a`. Khi thay đổi `b`, `a` vẫn giữ nguyên giá trị là `10`.

### 3. Ví dụ Reference Types

```csharp
class Student
{
    public string Name = "";
}

Student s1 = new Student();
s1.Name = "Nam";

Student s2 = s1;
s2.Name = "An";

Console.WriteLine(s1.Name); // An
```

**Giải thích:** `s1` và `s2` cùng tham chiếu một đối tượng. Khi thay đổi `Name` thông qua `s2`, dữ liệu đọc qua `s1` cũng thay đổi.

**Kết luận:** Value Types sao chép giá trị, còn Reference Types sao chép tham chiếu.

---

## Câu 2: Phân biệt `init` và `set` trong C#

* **`set`:** Cho phép thay đổi giá trị thuộc tính sau khi khởi tạo đối tượng.
* **`init`:** Chỉ cho phép gán giá trị trong quá trình khởi tạo đối tượng, không cho phép gán lại bằng cách thông thường sau đó.

### Ví dụ

```csharp
class Student
{
    public string Name { get; set; }
    public int StudentId { get; init; }
}

Student s = new Student
{
    Name = "Nam",
    StudentId = 123
};

s.Name = "An";       // Hợp lệ
// s.StudentId = 456; // Lỗi biên dịch
```

**Giải thích:** `Name` sử dụng `set` nên có thể thay đổi từ `"Nam"` thành `"An"`. `StudentId` sử dụng `init` nên không thể gán lại sau khi khởi tạo.

**Ứng dụng thực tế:** Dùng `init` cho mã sinh viên hoặc mã đơn hàng cần ổn định sau khi tạo đối tượng; dùng `set` cho thông tin cần chỉnh sửa thường xuyên.

**Kết luận:** `set` cho phép cập nhật dữ liệu, còn `init` giúp hạn chế việc thay đổi dữ liệu sau khi khởi tạo.

---

## Câu 3: Phân biệt `virtual` và `override` trong tính đa hình

* **`virtual`:** Khai báo ở lớp cha, cho phép lớp con ghi đè phương thức.
* **`override`:** Khai báo ở lớp con để cung cấp cách triển khai mới cho phương thức `virtual` hoặc `abstract` của lớp cha.

### Ví dụ

```csharp
class Animal
{
    public virtual void Sound()
    {
        Console.WriteLine("Animal sound");
    }
}

class Dog : Animal
{
    public override void Sound()
    {
        Console.WriteLine("Gau gau");
    }
}

Animal animal = new Dog();
animal.Sound(); // Gau gau
```

**Giải thích:** Lớp `Animal` khai báo phương thức `Sound()` bằng `virtual`. Lớp `Dog` ghi đè phương thức bằng `override`. Khi gọi `animal.Sound()`, chương trình thực hiện phương thức của đối tượng `Dog` thực tế.

**Kết luận:** `virtual` cho phép ghi đè, `override` thực hiện ghi đè. Đây là cơ chế hỗ trợ tính đa hình (Polymorphism) trong C#.

---

## Câu 4: Tại sao thành phần `static` không thể truy xuất thông qua Object Instance?

### 1. Khái niệm

`static` là từ khóa dùng để khai báo thành phần thuộc về lớp, thay vì thuộc về từng đối tượng. Vì vậy, thành phần `static` được truy cập thông qua tên lớp.

### 2. Ví dụ

```csharp
class Student
{
    public static int Count = 0;
    public string Name = "";
}

Student.Count = 2; // Hợp lệ

Student s = new Student();
s.Name = "Nam";

// Console.WriteLine(s.Count); // Lỗi biên dịch
Console.WriteLine(Student.Count); // 2
```

**Giải thích:**

* `Count` là biến `static`, được dùng chung ở cấp độ lớp `Student`.
* `Name` là biến thông thường, mỗi đối tượng có giá trị riêng.
* `Student.Count` hợp lệ vì truy cập thông qua tên lớp.
* `s.Count` không hợp lệ vì `Count` không phải thành phần riêng của đối tượng `s`.

**Ứng dụng thực tế:** `static` thường dùng cho biến đếm số lượng đối tượng, hàm tính toán tiện ích hoặc dữ liệu dùng chung.

**Kết luận:** Thành phần `static` thuộc về lớp nên được truy cập bằng tên lớp, không phải thông qua từng đối tượng.
