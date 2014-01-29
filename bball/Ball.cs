using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using AI;
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
            Dribbling,
            //Holding,
            //Intercepting,
            Passing,
            //Rolling,
            Shooting,
            //Stealing,
        }

        private CourtPos currentPos;
        private CourtPos beganPos;
        private CourtPos targetPos;
        private IImage image = null;
        private float scaleRate;
        private State state;

        public override void OnDraw(IRenderer r)
        {
            var ia = new ImageArgs(image);
            ia.CorrectToCenter = true;
            ia.Location = Court.ToGlobalLocation(currentPos).Location;
            ia.Scale = new Vector3f(scaleRate, scaleRate, scaleRate);
            r.PutImage(ia);
        }

        public override void OnUpdate()
        {
            switch (state)
            {
                case State.Bounding:    // 원래 scaleRate 값 유지(일단은 아무 일도 하지 않음)
                    break;
                case State.Passing:
                    var vDirect = this.targetPos - this.currentPos;
                    vDirect.Location.Normalize();
                    currentPos = this.currentPos + vDirect * (float)3;
                    break;
                case State.Shooting:
                    // Note : 아직 여러 가지 물리 지수가 없으므로 공의 속도, 뜨는 높이, 거리비는 대충 사용한다.
                    var distance = beganPos.DistanceTo(targetPos);
                    var floating = beganPos.DistanceTo(currentPos);
                    if (floating >= distance)
                    {
                        this.CurrentState = State.Bounding;
                    }
                    else
                    {
                        var remain = currentPos.DistanceTo(targetPos);
                        var highestPoint = distance / 2.0f + distance * 0.15f;
                        scaleRate = ((float)Math.Cos(MyMath.DegreeToRadian((highestPoint - remain) / highestPoint * 90))) * 0.4f + 0.4f;
                    }
                    currentPos += CourtPos.FromCoord(5, 0, 0);
                    break;
                case State.Dribbling:
                    break;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException("공의 상태 정의가 추가로 필요한 지점입니다.");
            }
        }

        public Ball()
        {
            this.CurrentState = State.Bounding;
        }

        public void Move()
        {
            var vDirect = this.targetPos - this.currentPos;
            vDirect.Location.Normalize();
            currentPos = this.currentPos + vDirect;
        }

        public IImage Image
        {
            get { return image; }
            set { image = value; }
        }

        public CourtPos Location
        {
            get { return currentPos; }
            set
            {
                switch(state)
                {
                    case State.Bounding:
                        currentPos = value;
                        break;
                    case State.Passing:
                        throw new Exception("이 상태에서는 위치를 강제 입력할 수 없습니다.");
                    case State.Shooting:
                        throw new Exception("이 상태에서는 위치를 강제 입력할 수 없습니다.");
                    case State.Dribbling:
                        currentPos = value;
                        break;
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
                    case State.Passing:
                        scaleRate = 0.5f;
                        beganPos = currentPos;
                        break;
                    case State.Shooting:
                        scaleRate = 0.5f;
                        beganPos = currentPos;
                        break;
                    case State.Dribbling:
                        scaleRate = 0.5f;
                        break;
                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException("공의 상태 정의가 추가로 필요한 지점입니다.");
                }
            }
        }

        public CourtPos TargetLocation
        {
            get { return targetPos; }
            set { targetPos = value; }
        }
    }
}
