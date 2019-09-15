namespace Rogue
{
	class Fov{
		public float[,] mask;
		private int width,height;
		private float intensity,ambient,falloff;
		public Fov(int w,int h){
			mask=new float[w,h];
			width=w;
			height=h;
			intensity=System.Single.MaxValue;
			falloff=1.0f/intensity;
			ambient=0.0f;
		}
		public void reset(){
			for(int i=0;i<width;i++)for(int j=0;j<height;j++)mask[i,j]=ambient;
		}
		public void addLight(int x,int y,float br){
			mask[x,y]=System.Math.Max(mask[x,y],br);
		}
		public void setIntensity(float i){
			i=System.Math.Max(1.0f,i);
			intensity=i;
			falloff=1.0f/i;
		}
		public void setAmbient(float a){
			ambient=System.Math.Max(0.0f,System.Math.Min(1.0f,a));
		}
		float computeIntensity(float here,float neighbor1,float neighbor2,float wall){
			float local_falloff=System.Math.Min(1.0f,falloff+(wall/10.0f));
			neighbor1=System.Math.Max(here,neighbor1);
			neighbor2=System.Math.Max(here,neighbor2);
			return System.Math.Max(0.0f,System.Math.Max(neighbor1,neighbor2)-local_falloff);
		}
		void forwardProp(float[,] walls){
			for(int x=1;x<width;x++){
				mask[x,0]=computeIntensity(mask[x,0],mask[x-1,0],0.0f,walls[x,0]);
			}
			for(int y=1;y<height;y++){
				mask[0,y]=computeIntensity(mask[0,y],mask[0,y-1],0.0f,walls[0,y]);
				for(int x=1;x<width;x++){
					mask[x,y]=computeIntensity(mask[x,y],mask[x-1,y],mask[x,y-1],walls[x,y]);
				}
			}
		}
		void backwardProp(float[,] walls){
			for(int x=width-2;x>=0;x--){
				int y=height-1;
				mask[x,y]=computeIntensity(mask[x,y],mask[x+1,y],0.0f,walls[x,y]);
			}
			for(int y=height-2;y>=0;y--){
				int x_=width-1;
				mask[x_,y]=computeIntensity(mask[x_,y],mask[x_,y+1],0.0f,walls[x_,y]);
				for(int x=width-2;x>=0;x--){
					mask[x,y]=computeIntensity(mask[x,y],mask[x+1,y],mask[x,y+1],walls[x,y]);
				}
			}
		}
		void blur(float[,] from,float[,] to,int rad){
			int numtiles=((2*rad)+1)^2;
			for(int i=rad;i<width-rad;i++){
				for(int j=rad;j<height-rad;j++){
					float sum=0.0f;
					for(int kx=i-rad;kx<=i+rad;kx++){
						for(int ky=j-rad;ky<=j+rad;ky++){
							sum+=from[kx,ky];
						}
					}
					float avg=sum/numtiles;
					to[i,j]=avg;
				}
			}
		}
		void computeMask(float[,] walls){
			for(int i=0;i<width;i++)for(int j=0;j<height;j++)mask[i,j]=System.Math.Max(0.0f,mask[i,j]-walls[i,j]);
			forwardProp(walls);
			backwardProp(walls);
			forwardProp(walls);
			backwardProp(walls);
			for(int i=0;i<width;i++)
				for(int j=0;j<height;j++)
				if(walls[i,j]>0.0f&&mask[i,j]>0.0f)
					mask[i,j]=System.Math.Min(1.0f,mask[i,j]+0.1f);
			for(int i=0;i<width;i++)
				for(int j=0;j<height;j++)
					if(walls[i,j]==0.0f)mask[i,j]=System.Math.Max(ambient,mask[i,j]);
		}
		float[,] map;
		public void load(Level level){
			map=new float[level.width,level.height];
			for(int x=0;x<level.width;x++){
				for(int y=0;y<height;y++){
					if(!level.isMove(x,y)){
						map[x,y]=System.Single.MaxValue;
					}else{
						map[x,y]=0.0f;
					}
				}
			}
		}
		void lightWalls(){
			for(int x=0;x<width;x++){
				for(int y=0;y<height;y++){
					if(map[x,y]!=0.0f)
						for(int dx=-1;dx<=1;dx++){
							for(int dy=-1;dy<=1;dy++){
								if(dx==dy&&dx==0)continue;
								if(x+dx>=width||y+dy>=height||dx+x<0||dy+y<0)continue;
								if(mask[x+dx,y+dy]!=0.0f&&map[x+dx,y+dy]==0.0f){
									mask[x,y]=mask[x+dx,y+dy];
									dx=2;
									dy=2;
								}
							}
						}
				}
			}
		}
		public void compute(System.Drawing.Point pos,float br){
			reset();
			addLight(pos.X,pos.Y,br);
			computeMask(map);
			lightWalls();
		}
	}
}