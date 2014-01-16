using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Core;
using Physics;
using Renderer;

namespace bball
{
    class Ball : Object
    {
        public enum State
        {
            Bounding,
            //Holding,
            //Intercepting,
            //Passing,
            //Rolling,
            Shooting,
            //Stealing,
        }

        private Point pos = new Point();
        private Point beganPos;
        private Point targetPos;
        private IImage image = null;
        private int oldTick = 0;
        private float scaleRate;
        private State state;

        public override void OnDraw(IRenderer r)
        {
            var pt = Court.LogicalCoordToPhysicalCoord(pos);
            var ia = new ImageArgs(image);
            ia.CorrectToCenter = true;
            ia.SetPos(pt.X, pt.Y);
            ia.SetScale(this.GetScaleRate());
            r.PutImage(ia);
        }

        public Ball()
        {
            this.CurrentState = State.Bounding;
        }

        private float GetScaleRate()
        {
            float rate = 0.0f;
            switch (state)
            {
                case State.Bounding:
                    rate = scaleRate;
                    break;
                case State.Shooting:
                    // Note : 아직 여러 가지 물리 지수가 없으므로 공의 속도, 뜨는 높이, 거리비는 대충 사용한다.
                    var distance = PhysicsEngine.GetDistance(beganPos, targetPos);
                    var floating = PhysicsEngine.GetDistance(pos, targetPos);
                    var highestPoint = distance / 2.0f + distance * 0.15f;
                    scaleRate = ((float)Math.Cos(MyMath.DegreeToRadian((highestPoint - floating) * 90))) * 0.3f + 0.5f;
                    break;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException("공의 상태 정의가 추가로 필요한 지점입니다.");
            }
            return rate;
        }

        public IImage Image
        {
            get { return image; }
            set { image = value; }
        }

        public Point Location
        {
            get { return pos; }
            set
            {
                switch(state)
                {
                    case State.Bounding:
                        pos = value;
                        break;
                    case State.Shooting:
                        throw new Exception("이 상태에서는 위치를 강제 입력할 수 없습니다.");
                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException("공의 상태 정의가 추가로 필요한 지점입니다.");
                }
            }
        }

        public State CurrentState
        {
            get { return state; }
            set
            {
                state = value;
                switch (state)
                {
                    case State.Bounding:
                        scaleRate = 0.5f;
                        break;
                    case State.Shooting:
                        scaleRate = 0.5f;
                        beganPos = pos;
                        break;
                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException("공의 상태 정의가 추가로 필요한 지점입니다.");
                }
            }
        }

        public Point TargetLocation
        {
            get { return targetPos; }
            set { targetPos = value; }
        }
    }
}
