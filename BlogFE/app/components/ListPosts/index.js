/**
 *
 * ListPosts
 *
 */

import React, { memo } from 'react';
import PropTypes from 'prop-types';
import { makeStyles } from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import ListItemText from '@material-ui/core/ListItemText';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import Checkbox from '@material-ui/core/Checkbox';
import Avatar from '@material-ui/core/Avatar';
import InfiniteScroll from 'react-infinite-scroller';
import IconButton from '@material-ui/core/IconButton';
import SearchIcon from '@material-ui/icons/Search';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Hidden from '@material-ui/core/Hidden';
import Link from '@material-ui/core/Link';
import config from 'config'; //eslint-disable-line

const useStyles = makeStyles(theme => ({
  root: {
    width: '100%',
    maxWidth: 360,
    backgroundColor: theme.palette.background.paper,
  },
  postCard: {
    display: 'flex',
    minHeight: '250px',
  },
  cardDetails: {
    flex: 1,
  },
  cardMedia: {
    width: 260,
  },
}));

function ListPosts({ posts, loadPost, hasMore }) {
  const classes = useStyles();

  return (
    <InfiniteScroll
      pageStart={1}
      loadMore={loadPost}
      hasMore={hasMore}
      initialLoad={false}
      loader={
        <div className="loader" key={0}>
          Loading ...
        </div>
      }
    >
      <Grid container spacing={4}>
        {posts &&
          posts.map(post => (
            <Grid item key={post.id} xs={12}>
              <CardActionArea
                component="a"
                href={`/post/${post.id}/${post.urlSlug}`}
              >
                <Card className={classes.postCard}>
                  <Hidden xsDown>
                    <CardMedia
                      className={classes.cardMedia}
                      image={`${config.baseApi}file/${post.fileUploadId}`}
                      title="Image title"
                    />
                  </Hidden>
                  <div className={classes.cardDetails}>
                    <CardContent>
                      <Typography component="h2" variant="h5">
                        {post.title}
                      </Typography>
                      <Typography variant="subtitle1" color="textSecondary">
                        {post.date}
                      </Typography>
                      <Typography variant="subtitle1" paragraph>
                        {post.shortDescription}
                      </Typography>
                      <Link
                        variant="subtitle1"
                        href={`/post/${post.id}/${post.urlSlug}`}
                      >
                        Continue reading...
                      </Link>
                    </CardContent>
                  </div>
                </Card>
              </CardActionArea>
            </Grid>
          ))}
      </Grid>
    </InfiniteScroll>
  );
}

ListPosts.propTypes = {
  posts: PropTypes.array,
  loadPost: PropTypes.func,
  hasMore: PropTypes.bool,
};

export default memo(ListPosts);
