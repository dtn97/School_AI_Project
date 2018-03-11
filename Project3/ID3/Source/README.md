Thông tin Solution bài 2: 

Source code gồm 1 file mã nguồn duy nhất alà main.cpp
	Ngôn ngữ: C++ 11 trở lên
	Do em không có visual studio nên Thầy/Cô khi chấm có thể dùng g++ để biên dịch ạ.

Một số lưu ý khi chạy: 
	Code cứng sữ dụng 3 file input được đặt trong thư mục cùng cấp với file thực thi
		train.txt: chứa dữ liệu train để xây dựng cây
		test.txt: chứa dữ liệu test
		compare.txt: chứa nhãn để kiểm tra kết quả

Khi thực thi, chương trình có hỏi thuộc tính muốn loại bỏ. 
Do bộ dữ liệu zoo có thuộc tính là aninal name, sẽ làm mất bản chất của việc tính Entropy và Gian Infomation nên cần loại bỏ thuộc tính này, nếu không loại bỏ thì thuộc tính animal name sẽ được dùng đầu tiên để chia cây.