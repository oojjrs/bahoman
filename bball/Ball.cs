﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using AI;
using Core;
using Renderer;

namespace bball
{
    class Ball : Object, IMouseHandler
    {
        private CourtPos currentPos;
        private CourtPos beganPos;
        private CourtPos targetPos;
        private float force = 1;
        private IImage image = null;
        private float scaleRate;
        private BallState state = BallState.Bounding;
        private Player thrower;
        private CourtPos shootingDirection;
        private CourtPos passDirection;
        private bool isSelected = false;

        public override void OnDraw(IRenderer r)
        {
            var ia = new ImageArgs(image);
            ia.CorrectToCenter = true;
            ia.Location = Court.ToGlobalLocation(currentPos).Vector;
            ia.Scale = new Vector3f(scaleRate, scaleRate, scaleRate);
            r.PutImage(ia);

            if(isSelected)
            {
                var ta = TextArgs.Create("선택됐음. 테스트 문구 출력 중", OutputManager.DefaultFont);
                ta.Rect = new Rectangle(Court.ToGlobalLocation(currentPos).ToPoint(), new Size(200, 20));
                r.PutText(ta);
            }
        }

        public override void OnUpdate()
        {
            switch (state)
            {
                case BallState.Bounding:    // 원래 scaleRate 값 유지(일단은 아무 일도 하지 않음)
                    break;
                case BallState.Passing:
                    currentPos = this.currentPos + this.Direction * this.Force;
                    break;
                case BallState.Shooting:
                    // Note : 아직 여러 가지 물리 지수가 없으므로 공의 속도, 뜨는 높이, 거리비는 대충 사용한다.
                    var distance = beganPos.DistanceTo(targetPos);
                    var floating = beganPos.DistanceTo(currentPos);
                    if (floating >= distance)
                    {
                        this.CurrentState = BallState.Bounding;
                    }
                    else
                    {
                        var remain = currentPos.DistanceTo(targetPos);
                        var highestPoint = distance / 2.0f + distance * 0.15f;
                        scaleRate = ((float)Math.Cos(MyMath.DegreeToRadian((highestPoint - remain) / highestPoint * 90))) * 0.4f + 0.4f;
                    }
                    currentPos += shootingDirection * 5;
                    break;
                case BallState.Dribbling:
                    break;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException("공의 상태 정의가 추가로 필요한 지점입니다.");
            }
        }

        public virtual bool OnMouse(MouseArgs e)
        {
            isSelected = !isSelected;
            return true;
        }

        public virtual Rectangle Zone
        {
            get
            {
                var rc = new Rectangle(Court.ToGlobalLocation(this.Location).ToPoint(), image.Size);
                rc.Offset(-image.Size.Width / 2, -image.Size.Height / 2);
                return rc;
            }
        }

        public Ball()
        {
            this.Image = ImageFactory.Create("res/Ball.png");
            this.Location = CourtPos.FromCoord(50, 0, 50);
            this.CurrentState = BallState.Bounding;

            InputManager.Add(this);
        }

        public void Move()
        {
            var vDirect = this.targetPos - this.currentPos;
            if (vDirect.Length <= 1.0f)
                return;
            vDirect = vDirect.Normalize;
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
                    case BallState.Bounding:
                        currentPos = value;
                        break;
                    case BallState.Passing:
                        throw new Exception("이 상태에서는 위치를 강제 입력할 수 없습니다.");
                    case BallState.Shooting:
                        throw new Exception("이 상태에서는 위치를 강제 입력할 수 없습니다.");
                    case BallState.Dribbling:
                        currentPos = value;
                        break;
                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException("공의 상태 정의가 추가로 필요한 지점입니다.");
                }
            }
        }

        public BallState CurrentState
        {
            get { return state; }
            set
            {
                state = value;
                switch (state)
                {
                    case BallState.Bounding:
                        scaleRate = 0.5f;
                        break;
                    case BallState.Passing:
                        scaleRate = 0.5f;
                        beganPos = currentPos;
                        break;
                    case BallState.Shooting:
                        scaleRate = 0.5f;
                        beganPos = currentPos;
                        shootingDirection = (targetPos - beganPos).Normalize;
                        break;
                    case BallState.Dribbling:
                        scaleRate = 0.5f;
                        break;
                    default:
                        throw new System.ComponentModel.InvalidEnumArgumentException("공의 상태 정의가 추가로 필요한 지점입니다.");
                }
            }
        }

        public float Force
        {
            get { return force; }
            set { force = value; }
        }

        public CourtPos TargetLocation
        {
            get { return targetPos; }
            set { targetPos = value; }
        }

        public CourtPos Direction
        {
            get { return passDirection; }
            set { passDirection = value; }
        }

        public Player Thrower
        {
            get { return thrower; }
            set { thrower = value; }
        }
    }
}
