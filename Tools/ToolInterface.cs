using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public interface ToolInterface
    {
        // 툴 초기화
        int SetImage(Cognex.VisionPro.Display.CogDisplay display);
        int SetInfo(string Item, string Part, string toolName);

        void SetViewLabel(bool flag);

        // 툴 설정
        int Confirm();
        int Cancle();

        // 툴 실행
        int Run();

        // 결과값 불러오기
        string GetResult();

        // 툴 해재
        void Release();

    }
}
